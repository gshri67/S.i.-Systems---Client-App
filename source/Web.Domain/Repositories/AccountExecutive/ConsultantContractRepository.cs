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
        ContractSummarySet GetFlowThruSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id);
    }

    public class ConsultantContractRepository : IConsultantContractRepository
    {
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
                  /*,
                                            CAST(agr.StatusType AS INT) StatusType,
                                            agrRateDetail.BillRate Rate,
                                            spec.Name SpecializationName, 
                                            spec.Description SpecializationNameShort,
                                            contact.UserID Id,
											contact.FirstName,
											contact.LastName,
											contactEmail.PrimaryEmail EmailAddress,
                                            directReport.UserID Id,
	                                        directReport.FirstName,
	                                        directReport.LastName,
	                                        directReportEmail.PrimaryEmail EmailAddress
                                            FROM [Agreement] agr 
                                            LEFT JOIN [Agreement_ContractDetail] agrDetail on agr.AgreementID = agrDetail.AgreementID
                                            LEFT JOIN [Agreement_ContractRateDetail] agrRateDetail on agr.AgreementID = agrRateDetail.AgreementID
                                            LEFT JOIN [Specialization] spec on agrDetail.SpecializationID = spec.SpecializationID
                                            JOIN [Users] contact on contact.UserID = agr.ContactID
		                                    JOIN [User_Email] contactEmail on contactEmail.UserID = contact.UserID
		                                    JOIN [Agreement_ContractAdminContactMatrix] m on m.AgreementId = agr.AgreementId and m.Inactive = 0
		                                    JOIN [Users] directReport on directReport.UserID = m.DirectReportUserID
		                                    JOIN [User_Email] directReportEmail on directReportEmail.UserID = directReport.UserID
					                        
                   
                                            WHERE agr.CandidateId = @UserId
						                        AND agr.AgreementType = @CONTRACT
                                                AND agr.AgreementSubType IN (@FLOTHRU, @FULLYSOURCED) -- fully sourced will be filtered out later for alumni
                                            ORDER BY agr.EndDate desc";*/

                
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
    }
}
