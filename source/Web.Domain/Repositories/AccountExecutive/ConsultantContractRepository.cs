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

        public ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numActive = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberActiveFloThruContractsQuery, new { Id = id }).FirstOrDefault();

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
                var numActive = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberActiveFullySourcedContractsQuery, new { Id = id }).FirstOrDefault();

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

        private const string ActiveContractsFilter = @"AND PickList.Title = 'Active'";

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

        private static readonly Contractor StandardMarketer = new Contractor
        {
            Id = 1,
            FirstName = "Jon",
            LastName = "Marketer",
            EmailAddresses = new List<string>() { "jm@email.com" }.AsEnumerable(),
            PhoneNumbers = Enumerable.Empty<string>(),
            Rating = MatchGuideConstants.ResumeRating.Standard
        };

        private static readonly UserContact ContactJanice = new UserContact
        {
            FirstName = "Janice",
            LastName = "McContact",
            EmailAddresses = new List<string>(){ "cc@email.com" }.AsEnumerable(),
            PhoneNumbers = new List<string>(){ "(555)555-1234" }.AsEnumerable()
        };

        private static readonly UserContact BillingContactWilliam = new UserContact
        {
            FirstName = "William",
            LastName = "Payerson",
            EmailAddresses = new List<string>(){ "will.payerson@email.com" }.AsEnumerable(),
            PhoneNumbers = new List<string>(){ "(555)555-4321" }.AsEnumerable()
        };

        private static readonly UserContact DirectReportCandice = new UserContact
        {
            FirstName = "Candice",
            LastName = "Consulty",
            EmailAddresses = new List<string>(){ "candice.consulty@email.com" }.AsEnumerable(),
            PhoneNumbers = new List<string>(){ "(555)555-9876" }.AsEnumerable()
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
