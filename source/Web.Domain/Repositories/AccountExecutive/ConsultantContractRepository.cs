using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IConsultantContractRepository
    {
        IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id);
        ConsultantContract GetContractDetailsById(int id);
        IEnumerable<ConsultantContract> GetContractsByContractorId(int id);
        int SubmitContract(Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId, string candidateEmail, int internalUserId);
        int SubmitRateTermDetailsForJob(User user, int agreementId, Rate rate, DateTime startDate, DateTime endDate);
    }

    public class ConsultantContractRepository : IConsultantContractRepository
    {
        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractSummaryQuery = @"SELECT Agreement.AgreementID AS ContractId,
	                                                            Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                                                            Company.CompanyName AS ClientName,
	                                                            Details.JobTitle AS Title,
	                                                            Agreement.StartDate,
	                                                            Agreement.EndDate,
	                                                            CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType
                                                            FROM Agreement
                                                            JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                                                            JOIN Users AE ON Agreement.AccountExecID = AE.UserID
                                                            JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                                                            JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                                                            JOIN Company ON Agreement.CompanyID = Company.CompanyID
                                                            WHERE Agreement.AccountExecID = @Id
                                                            AND Agreement.AgreementType IN (
	                                                            SELECT PickListId FROM udf_GetPickListIds('agreementtype', 'contract', 4)
                                                            )
                                                            AND AE.verticalid = 4
                                                            AND ISNULL(Details.PreceedingContractID, 0) = 0 --Omit Renewals
                                                            AND Agreement.AgreementSubType IN (
	                                                            SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
	                                                            UNION
	                                                            SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
                                                            )
                                                            AND (
	                                                            PickList.Title = 'Active'
	                                                            OR DATEDIFF(day, GetDate(), Agreement.StartDate) BETWEEN 0 AND 30 
                                                            )";


                var contracts = db.Connection.Query<ConsultantContractSummary>(contractSummaryQuery, new { Id = id });

                return contracts;
            }
        }

        
        public int GetNumberOfActiveFloThruContracts(DateTime startDate )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Dashboard_PortFolio_ActiveFlothruContracts] 
                        @aContractStartMonth,
                        @aContractStartYear";

                var result = db.Connection.Query<int>(query, new
                {
                    aContractStartMonth = startDate.Month,
                    aContractStartYear = startDate.Year

                }).FirstOrDefault();

                return result;
            }
        }
        public int GetNumberOfActiveFullySourcedContracts(DateTime startDate)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Dashboard_PortFolio_ActiveContracts] 
                        @aContractStartMonth,
                        @aContractStartYear";

                var result = db.Connection.Query<int>(query, new
                {
                    aContractStartMonth = startDate.Month,
                    aContractStartYear = startDate.Year

                }).FirstOrDefault();

                return result;
            }
        }
        /*
        public int GetNumberOfActiveFloThruContracts(int userId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"SELECT Count(*)
                        FROM Agreement
                        JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                        JOIN Users ON Agreement.AccountExecID = Users.UserID
                        JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                        WHERE Agreement.AccountExecID = @UserId
                        AND Agreement.AgreementType IN (
                        SELECT PickListId FROM udf_GetPickListIds('agreementtype', 'contract', 4)
                        )
                        AND Users.verticalid = 4
                        AND ISNULL(Details.PreceedingContractID, 0) = 0 --Omit Renewals
 
                        AND Agreement.AgreementSubType IN (
                        SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
                        )
                        AND PickList.Title = 'Active'
                        AND DATEDIFF(day, GetDate(), Agreement.EndDate) > 30
                        ";

                var result = db.Connection.Query<int>(query, new
                {
                    UserId = userId

                }).FirstOrDefault();

                return result;
            }
        }*/

        public ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numActive = GetNumberOfActiveFloThruContracts(new DateTime(2015, 1, 1));//GetNumberOfActiveFloThruContracts( id );//db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberActiveFloThruContractsQuery, new { Id = id }).FirstOrDefault();

                var numEnding = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberEndingFloThruContractsQuery, new { Id = id }).FirstOrDefault();

                var numStarting = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberStartingFloThruContractsQuery, new { Id = id }).FirstOrDefault();

                return new ContractSummarySet
                {
                    Current = numActive,
                    Starting = numStarting,
                    Ending = numEnding
                };
            }
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numActive = GetNumberOfActiveFullySourcedContracts(new DateTime(2015, 1, 1));//db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberActiveFullySourcedContractsQuery, new { Id = id }).FirstOrDefault();

                var numEnding = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberEndingFullySourcedContractsQuery, new { Id = id }).FirstOrDefault();

                var numStarting = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberStartingFloThruContractsQuery, new { Id = id }).FirstOrDefault();
                
                return new ContractSummarySet
                {
                    Current = numActive,
                    Starting = numStarting,
                    Ending =  numEnding
                };
            }
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.AgreementID AS ContractId,
	                    Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                    Company.CompanyName AS ClientName,
	                    Details.JobTitle AS Title,
	                    Agreement.StartDate,
	                    Agreement.EndDate,
	                    CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType,
	                    Company.CompanyID ClientId,
	                    RateDetails.BillRate,
	                    RateDetails.PayRate,
	                    RateDetails.BillRate - RateDetails.PayRate AS GrossMargin
                    FROM Agreement
                    JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                    JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                    JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    JOIN Agreement_ContractRateDetail RateDetails on Agreement.AgreementID = RateDetails.AgreementID
                    WHERE Agreement.AgreementID = @Id";

                var contract = db.Connection.Query<ConsultantContract>(contractsQuery, param: new { Id = id }).FirstOrDefault();

                return contract;
            }
        }

        public IEnumerable<ConsultantContract> GetContractsByContractorId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.AgreementID AS ContractId,
	                    Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                    Company.CompanyName AS ClientName,
	                    Details.JobTitle AS Title,
	                    Agreement.StartDate,
	                    Agreement.EndDate,
	                    CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType,
	                    Company.CompanyID ClientId,
	                    RateDetails.BillRate,
	                    RateDetails.PayRate,
	                    RateDetails.BillRate - RateDetails.PayRate AS GrossMargin
                    FROM Agreement
                    JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                    JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                    JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    JOIN Agreement_ContractRateDetail RateDetails on Agreement.AgreementID = RateDetails.AgreementID
                    WHERE Agreement.CandidateID = @Id";

                var contracts = db.Connection.Query<ConsultantContract>(contractsQuery, param: new { Id = id });

                return contracts;
            }
        }

        public int SubmitContract( Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId, string candidateEmail, int internalUserId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[USPSetCreateNewFSContractInsert] 
                           @opportunityid
                          ,@internaluserid
                          ,@startdate
                          ,@enddate
                          ,@accountexec
                          ,@revenueaccountexec
                          ,@Commissionaccountexec
                          ,@contactuserid
                          ,@candidateuserid
                          ,@candidateemail
                          ,@timefactortype
                          ,@paymentplantype
                          ,@cancellation
                          ,@jobtitle
                          ,@activetimesheet
                          ,@activeproject
                          ,@expenditure
                          ,@expenseexpenditure
                          ,@contractcandidatename
                          ,@contractclientcontactname
                          ,@TimeSheetType
                          ,@IsThirdPartyBilling
                          ,@IsThridPartyPay
                          ,@ThirdPartyBillingCompany
                          ,@ThirdPartyPayCandidate
                          ,@InvoiceFrequency
                          ,@CandidatePaymentType
                          ,@VerticalID
                          ,@Limitationofcontracttype
                          ,@InvoiceFormatId
                          ,@HasProjectPO
                          ,@Quickpay
                          ,@SAID";

                var result = db.Connection.Query<int>(query, new
                {
                    opportunityid = job.Id,
                    internaluserid = internalUserId,
                    startdate = contract.StartDate,
                    enddate = contract.EndDate,
                    accountexec = contract.AccountExecutive.Id,
                    revenueaccountexec = contract.BillingContact.Id,
                    Commissionaccountexec = contract.ComissionAssigned.Id,
                    contactuserid = job.ClientContact.Id,
                    candidateuserid = candidateUserId,
                    candidateemail = candidateEmail,
                    timefactortype = contract.TimeFactor,
                    paymentplantype = contract.PaymentPlan,
                    cancellation = contract.DaysCancellation,
                    jobtitle = job.Title,
                    activetimesheet = timesheet.Id,
                    activeproject = 1,
                    expenditure = contract.LimitationExpense,
                    expenseexpenditure = contract.LimitationExpense,
                    contractcandidatename = job.ClientName,
                    contractclientcontactname = job.ClientContact.ClientName,
                    TimeSheetType = MatchGuideConstants.TimesheetType.ETimesheet,
                    IsThirdPartyBilling = false,
                    IsThridPartyPay = false,
                    ThirdPartyBillingCompany = 0,
                    ThirdPartyPayCandidate = 0,
                    InvoiceFrequency = contract.InvoiceFrequency,
                    CandidatePaymentType = 714,
                    VerticalID = MatchGuideConstants.VerticalId.IT,
                    Limitationofcontracttype = contract.LimitationOfContractType,
                    //InvoiceFormatId = ,//generally == 9
                    HasProjectPO = false,
                    Quickpay = contract.UsingQuickPay,
                    //SAID = //is this the association id?
                }).FirstOrDefault();

                return result;
            }
        }

        public int SubmitAdminContactDetailsForJob( int agreementId, int internalUserId, int directReportId, int billingContactId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_CON_Contract_AdminContact_Insert] 
                           @agreementid
                          ,@internaluserid
                          ,@directreport
                          ,@billingcontact
                          ,@VerticalID";

                var result = db.Connection.Query<int>(query, new
                {
                    agreementid = agreementId,
                    internaluserid = internalUserId,
                    directreport = directReportId,
                    billingContact = billingContactId,
                    VerticalID = MatchGuideConstants.VerticalId.IT

                }).FirstOrDefault();

                return result;
            }
        }

   
        public int SubmitRateTermDetailsForJob(User user, int agreementId, Rate rate, DateTime startDate, DateTime endDate )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_CON_Contract_RateTerm_Insert] 
                       @internaluserid
                      ,@ratetermtype
                      ,@nbrhours
                      ,@description
                      ,@billrate
                      ,@payrate
                      ,@startdate
                      ,@enddate
                      ,@agreementid
                      ,@header
                      ,@comments
                      ,@activityTypename
                      ,@IsPrimary
                      ,@PAMRateID
                      ,@InActive
                      ,@AdminFee
                      ,@VerticalID";

                var result = db.Connection.Query<int>(query, new
                {
                    internaluserid = user.Id,
                    ratetermtype = rate.RateType,
                    nbrhours = rate.HoursPerDay,
                    description = rate.Description,
                    billrate = rate.BillRate,
                    payrate = rate.PayRate,
                    startdate = startDate,
                    enddate = endDate,
                    agreementid = agreementId,
                    header = string.Format( "Contract Created by {0}", user.FullName),
                    comments = string.Format("Rate Term for Contract ID"),
                    activityTypename = "ContractRateTerm",
                    IsPrimary = rate.IsPrimaryRate,
                    PAMRateID = 0,
                    InActive = false,
                    AdminFee = 0,
                    VerticalID = MatchGuideConstants.VerticalId.IT

                }).FirstOrDefault();

                return result;
            }
        }
    }

    internal static class AccountExecutiveContractsQueries
    {
        private const string NumberOfContractsForAccountExecutiveQuery = @"SELECT Count(*)
                                                FROM Agreement
                                                JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                                                JOIN Users ON Agreement.AccountExecID = Users.UserID
                                                JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                                                WHERE Agreement.AccountExecID = @Id
                                                AND Agreement.AgreementType IN (
	                                                SELECT PickListId FROM udf_GetPickListIds('agreementtype', 'contract', 4)
                                                )
                                                AND Users.verticalid = 4
                                                AND ISNULL(Details.PreceedingContractID, 0) = 0 --Omit Renewals";

        private const string FullySourcedFilter = @"AND Agreement.AgreementSubType IN (
	                                                SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
                                                )";
        private const string FloThruFilter = @"AND Agreement.AgreementSubType IN (
	                                                SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
                                                )";

        private const string ActiveContractsFilter = @"AND PickList.Title = 'Active'
            AND DATEDIFF(day, GetDate(), Agreement.EndDate) > 30";

        private const string EndingContractsFilter =
            @" AND DATEDIFF(day, GetDate(), Agreement.EndDate) BETWEEN 0 AND 30 -- Ending Contracts";

        private const string StartingContractsFilter =
            @" AND DATEDIFF(day, GetDate(), Agreement.StartDate) BETWEEN 0 AND 30 -- Starting Contracts";

        public static string NumberActiveFullySourcedContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}", 
                    Environment.NewLine,
                    NumberOfContractsForAccountExecutiveQuery, 
                    FullySourcedFilter,
                    ActiveContractsFilter);
            }
        }

        public static string NumberStartingFullySourcedContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}",
                    Environment.NewLine, 
                    NumberOfContractsForAccountExecutiveQuery, 
                    FullySourcedFilter,
                    StartingContractsFilter);
            }
        }

        public static string NumberEndingFullySourcedContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}",
                    Environment.NewLine,
                    NumberOfContractsForAccountExecutiveQuery, 
                    FullySourcedFilter,
                    EndingContractsFilter);
            }
        }

        public static string NumberActiveFloThruContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}",
                    Environment.NewLine,
                    NumberOfContractsForAccountExecutiveQuery, 
                    FloThruFilter,
                    ActiveContractsFilter);
            }
        }

        public static string NumberStartingFloThruContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}",
                    Environment.NewLine,
                    NumberOfContractsForAccountExecutiveQuery, 
                    FloThruFilter,
                    StartingContractsFilter);
            }
        }

        public static string NumberEndingFloThruContractsQuery
        {
            get
            {
                return string.Format("{1} {0} {2} {0} {3}",
                    Environment.NewLine,
                    NumberOfContractsForAccountExecutiveQuery, 
                    FloThruFilter,
                    EndingContractsFilter);
            }
        }
    }

    public class MockedConsultantContractRepository : IConsultantContractRepository
    {
        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            
            return new List<ConsultantContractSummary>
            {
                FredAtNexenActiveFloThruContract,FredAtNexenEndingFloThruContract, FredAtNexenEndingFullySourcedContract,
                BarneyAtNexenActiveFloThruContract, BarneyAtNexenStartingFloThruContract, BarneyAtNexenStartingFullySourcedContract,
                BamBamAtCenovusActiveFloThruContract, BamBamAtCenovusActiveFullySourcedContract
            }.AsEnumerable();
        }

        private ConsultantContractSummary FredAtNexenActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 1,
            ClientName = "Nexen",
            ContractorName = "Fred Flintstone",
            Title = string.Format("{0} - Job title with indepth description to indicate length", 60123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-2).AddDays(-14),
            EndDate = DateTime.UtcNow.AddMonths(2).AddDays(10)
        };

        private ConsultantContractSummary BarneyAtNexenActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 2,
            ClientName = "Nexen",
            ContractorName = "Barney Rubble",
            Title = string.Format("{0} - Project Manager", 59326),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddDays(-15),
            EndDate = DateTime.UtcNow.AddMonths(3).AddDays(5)
        };

        private ConsultantContractSummary BamBamAtCenovusActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 3,
            ClientName = "Cenovus",
            ContractorName = "BamBam Rubble",
            Title = string.Format("{0} - Solution Architect", 59950),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(2)
        };

        private ConsultantContractSummary FredAtNexenEndingFloThruContract = new ConsultantContractSummary
        {
            ContractId = 4,
            ClientName = "Nexen",
            ContractorName = "Fred Flantstone",
            Title = string.Format("{0} - Telecom Analyst/ Implementation Specialist", 63321),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-7),
            EndDate = DateTime.UtcNow.AddDays(10)
        };

        private ConsultantContractSummary BarneyAtNexenStartingFloThruContract = new ConsultantContractSummary
        {
            ContractId = 5,
            ClientName = "Nexen",
            ContractorName = "Barney Rabble",
            Title = string.Format("{0} - Project Analyst", 62123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddDays(15),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        private ConsultantContractSummary BamBamAtCenovusActiveFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 6,
            ClientName = "Cenovus",
            ContractorName = "BamBam Rabble",
            Title = string.Format("{0} - ASP.NET MVC Developer", 59950),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(2)
        };

        private ConsultantContractSummary FredAtNexenEndingFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 7,
            ClientName = "Nexen",
            ContractorName = "Fred Flantstone",
            Title = string.Format("{0} - Job title with indepth description to indicate length", 63321),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddMonths(-3),
            EndDate = DateTime.UtcNow.AddDays(15)
        };

        private ConsultantContractSummary BarneyAtNexenStartingFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 8,
            ClientName = "Nexen",
            ContractorName = "Barney Rabble",
            Title = string.Format("{0} - Project Analyst", 62123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddDays(15),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        public ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 3,
                Starting = 1,
                Ending = 1
            };
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 1,
                Starting = 1,
                Ending = 1
            };
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            return ActiveMarketerAtNexenContract;
        }

        public IEnumerable<ConsultantContract> GetContractsByContractorId(int id)
        {
            return new List<ConsultantContract> {ActiveMarketerAtNexenContract};
        }

        public int SubmitContract(Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId, string candidateEmail, int internalUserId)
        {
            throw new NotImplementedException();
        }

        public int SubmitRateTermDetailsForJob(User user, int agreementId, Rate rate, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private static readonly Contractor StandardMarketer = new Contractor
        {
            ContactInformation = new UserContact
            {
                Id = 1,
                FirstName = "Jon",
                LastName = "Marketer",
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "jm@email.com", Title = "Primary" } }.AsEnumerable(),
                PhoneNumbers = Enumerable.Empty<PhoneNumber>()
            },
            Rating = MatchGuideConstants.ResumeRating.Standard
        };

        private static readonly UserContact ContactJanice = new UserContact
        {
            FirstName = "Janice",
            LastName = "McContact",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "cc@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly UserContact BillingContactWilliam = new UserContact
        {
            FirstName = "William",
            LastName = "Payerson",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "will.payerson@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly UserContact DirectReportCandice = new UserContact
        {
            FirstName = "Candice",
            LastName = "Consulty",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "candice.consulty@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly ConsultantContract ActiveMarketerAtNexenContract = new ConsultantContract
        {
            ContractId = 1,
            ClientName = "Nexen",
            ClientId = 1,
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            PayRate = 125,
            BillRate = 150,
            GrossMargin = 50,
            Title = "123456 - Telecom Analyst/ Implementation Specialist",
            Contractor = StandardMarketer,
            ClientContact = ContactJanice,
            BillingContact = BillingContactWilliam,
            DirectReport = DirectReportCandice,
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }
}
