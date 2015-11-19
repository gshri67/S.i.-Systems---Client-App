using System;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<ConsultantContract> GetContracts();
        IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFlowThruSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id);
        ConsultantContract GetContractDetailsById(int id);
    }

    public class ConsultantContractRepository : IConsultantContractRepository
    {
        private static readonly string Constants = @"DECLARE @ACTIVE int = " + MatchGuideConstants.ContractStatusTypes.Active + ","
                            + "@PENDING int = " + MatchGuideConstants.ContractStatusTypes.Pending + ","
                            + "@FLOTHRU int = " + MatchGuideConstants.AgreementSubTypes.FloThru + ","
                            + "@FULLYSOURCED int = " + MatchGuideConstants.AgreementSubTypes.Consultant + ","
                            + "@CONTRACT int = " + MatchGuideConstants.AgreementTypes.Contract + ","
                            + "@NOTCHECKED int = " + MatchGuideConstants.ResumeRating.NotChecked;

        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractSummaryQuery = @"SELECT agr.AgreementID as ContractId,
	                                                    consultant.FirstName as FirstName,
	                                                    consultant.LastName as LastName,
                                                        Company.CompanyName,
	                                                    agrDetail.JobTitle Title,
	                                                    agr.StartDate StartDate,
	                                                    agr.EndDate EndDate,
	                                                    agr.AgreementSubType
                                                    FROM [Agreement] agr
                                                    LEFT JOIN [Users] consultant ON consultant.UserID = agr.CandidateID
                                                    LEFT JOIN [Agreement_ContractDetail] agrDetail ON agr.AgreementID = agrDetail.AgreementID
                                                    LEFT JOIN [Company]  ON agr.CompanyID = company.CompanyID
                                                    WHERE agr.StatusType = @ACTIVE
                                                    OR agr.StatusType = @PENDING
                                                    ORDER BY agr.EndDate desc";


                var contracts = db.Connection.Query<ConsultantContractSummary>(Constants + contractSummaryQuery, new { Id = id });

                return contracts;
            }
        }

        public IEnumerable<ConsultantContract> GetContracts()
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string constants = @"DECLARE @ACTIVE int = " + MatchGuideConstants.ContractStatusTypes.Active + ","
                            + "@PENDING int = " + MatchGuideConstants.ContractStatusTypes.Pending + ","
                            + "@FLOTHRU int = " + MatchGuideConstants.AgreementSubTypes.FloThru + ","
                            + "@FULLYSOURCED int = " + MatchGuideConstants.AgreementSubTypes.Consultant + ","
                            + "@CONTRACT int = " + MatchGuideConstants.AgreementTypes.Contract + ","
                            + "@NOTCHECKED int = " + MatchGuideConstants.ResumeRating.NotChecked;

              const string query =
                        @"SELECT ContractRateID AS Id
	                        ,RateDescription AS RateDescription
                            ,PayRate AS Rate	
                        FROM Agreement_ContractRateDetail Dets";


              /*
               * agr.CandidateID ConsultantId,
                                          agr.CompanyID ClientId,
                                            
               */

              /*
               usr.FirstName consultant.FirstName
                usr.LastName consultant.LastName
               */

                const string contractsQuery =
                                            @"SELECT agr.CandidateID ConsultantId,
                                            agr.CompanyID ClientId,
	                                        agrDetail.JobTitle Title,
	                                        agr.StartDate StartDate,
	                                        agr.EndDate EndDate,
											company.CompanyName CompanyName,
	                                        directReport.FirstName FirstName,
                                            directReport.LastName LastName,
                                            directReportEmail.PrimaryEmail EmailAddress
                                            FROM [Agreement] agr
	                                        LEFT JOIN [Users] directReport ON agr.CandidateID = directReport.UserID
                                            LEFT JOIN [Agreement_ContractDetail] agrDetail ON agr.AgreementID = agrDetail.AgreementID
                                            JOIN [User_Email] directReportEmail on directReportEmail.UserID = directReport.UserID
											LEFT JOIN [Company] company ON agr.CompanyID = company.CompanyID
                                            ORDER BY agr.EndDate desc";

                
              var contracts = db.Connection.Query<ConsultantContract, ClientContact, ConsultantContract>(constants + contractsQuery,
                  (contract, directReport) =>
                  {
                      contract.DirectReport = directReport;
                      return contract;
                  }, splitOn: "FirstName");

                return contracts;
             }
        }

        public ContractSummarySet GetFlowThruSummaryByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class MockedConsultantContractRepository : IConsultantContractRepository
    {
        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            var oneWeekFromNow = DateTime.UtcNow.AddDays(7);
            var twoMonthsFromNow = DateTime.UtcNow.AddMonths(2);

            var summaries = new List<ConsultantContractSummary>();
            for (var i = 1; i < 30; i++)
            {
                DateTime startDate;
                DateTime endDate;
                if (i % 3 == 0)
                {
                    //starting contract
                    startDate = oneWeekFromNow;
                    endDate = twoMonthsFromNow;
                }
                else if (i % 3 == 1)
                {
                    //ending
                    startDate = oneWeekAgo;
                    endDate = oneWeekFromNow;
                }
                else
                {
                    //active
                    startDate = oneWeekAgo;
                    endDate = twoMonthsFromNow;
                }
                summaries.Add(new ConsultantContractSummary
                {
                    ContractId = i,
                    CompanyName = "Nexen",
                    ContractorName = "Fred Flintstone",
                    Title = string.Format("{0} - Job title with indepth description to indicate length", 321 * i),
                    AgreementSubType = i % 3 == 0 ? MatchGuideConstants.AgreementSubTypes.Consultant : MatchGuideConstants.AgreementSubTypes.FloThru,
                    StartDate = startDate,
                    EndDate = endDate
                });
            }
            return summaries;
        }

        public IEnumerable<ConsultantContract> GetContracts()
        {
            throw new NotImplementedException();
        }

        public ContractSummarySet GetFlowThruSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 55,
                Starting = 17,
                Ending = 12
            };
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 30,
                Starting = 15,
                Ending = 10
            };
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            return ActiveMarketerAtNexenContract;
        }

        private static readonly IM_Consultant StandardMarketer = new IM_Consultant
        {
            Id = 1,
            FirstName = "Jon",
            LastName = "Marketer",
            EmailAddress = "jm@email.com",
            Rating = MatchGuideConstants.ResumeRating.Standard
        };

        private static readonly ClientContact ContactJanice = new ClientContact
        {
            FirstName = "Janice",
            LastName = "McContact",
            EmailAddress = "cc@email.com",
            phoneNumber = "(555)555-1234"
        };

        private static readonly ClientContact BillingContactWilliam = new ClientContact
        {
            FirstName = "William",
            LastName = "Payerson",
            EmailAddress = "will.payerson@email.com",
            phoneNumber = "(555)555-4321"
        };

        private static readonly ClientContact DirectReportCandice = new ClientContact
        {
            FirstName = "Candice",
            LastName = "Consulty",
            EmailAddress = "candice.consulty@email.com",
            phoneNumber = "(555)555-9876"
        };

        private static readonly ConsultantContract ActiveMarketerAtNexenContract = new ConsultantContract
        {
            ContractId = 1,
            CompanyName = "Nexen",
            ClientId = 1,
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            PayRate = 125,
            BillRate = 150,
            GrossMargin = 50,
            Title = "123456 - Contract with a Title that is not too long",
            ConsultantId = 1,
            consultant = StandardMarketer,
            ClientContact = ContactJanice,
            BillingContact = BillingContactWilliam,
            DirectReport = DirectReportCandice,
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }
}
