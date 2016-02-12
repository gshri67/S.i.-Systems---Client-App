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

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface IPayRateRepository
    {
        IEnumerable<PayRate> GetPayRates();
        IEnumerable<int> GetContractRateIdFromTimesheet(Timesheet timesheet);
        IEnumerable<int> GetProjectIdFromTimesheet(Timesheet timesheet);
        IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(string projectCodeIds, string payRateIds,
            int verticalId);
    }

    public class PayRateRepository : IPayRateRepository
    {
        public IEnumerable<PayRate> GetPayRates()
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
              const string query =
                        @"SELECT ContractRateID AS Id
	                        ,RateDescription AS RateDescription
                            ,PayRate AS Rate	
                        FROM Agreement_ContractRateDetail Dets";
                    

                var payRates = db.Connection.Query<PayRate>(query);

                return payRates;
             }
        }

        public IEnumerable<int> GetContractRateIdFromTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                /*
                 DECLARE @RC int
                        EXECUTE @RC = [dbo].[UspGetRateTermDetails_TSAPP]
                        @AgreementId int,
	                    @TimesheetID int =0,
	                    @StartDate datetime=null,
	                    @Enddate datetime=null,
	                    @TimesheetStatus varchar(10)='None'
                 */

                const string query =
                    @"Declare @tablevar table(ContractRateID INT, PrimaryRateTerm BIT, RateDescription VARCHAR(250), HoursPerDay DECIMAL, BillRate MONEY, PayRate MONEY)
                        insert into @tablevar(ContractRateID, PrimaryRateTerm, RateDescription, HoursPerDay, BillRate, PayRate) EXECUTE [dbo].[UspGetRateTermDetails_TSAPP] 
                           @AgreementId
                          ,@TimesheetID
                          ,@StartDate
                          ,@Enddate
                          ,@TimesheetStatus

                        SELECT tempTable.ContractRateID as ContractID FROM @tablevar tempTable";



                var rateId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.ContractId,
                    TimesheetID = timesheet.Id,
                    StartDate = timesheet.StartDate.ToString("MMM d, YYYY"),
                    EndDate = timesheet.EndDate.ToString("MMM d, YYYY"),
                    TimesheetStatus = timesheet.Status
                });

                return rateId;
            }
        }

        public IEnumerable<int> GetProjectIdFromTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[UspGetProjectPODetails]
                        @AgreementId";

                var projectId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.ContractId
                });

                return projectId;
            }
        }

        public IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(string projectCodeIds, string payRateIds, int verticalId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                /*
                 * DECLARE @RC int
                        EXECUTE @RC = [dbo].[UspGetProjectPORateDetails_TSAPP]
                        @contractProjectPOIDList VARCHAR(1000),
	                    @contractRateTermIDList  VARCHAR(1000),
	                    @verticalid int,
                        @ProjectDescription OUTPUT,
                        @rateAmount OUTPUT,
                        @ratetype OUTPUT
                 */

                const string query =
                    @"Declare @tablevar table(AgreementId INT, ProjectId INT, EinvoiceId INT, ProjectDescription VARCHAR(250), DisplayProjectID VARCHAR(250), CompanyProjectID INT, ContractProjectPOID INT, POId INT, PODescription VARCHAR(250), DisplayPONumber VARCHAR(250), PONumber VARCHAR(250), companypoprojectmatrixid INT, contractrateid INT, primaryrateterm BIT, ratedescription VARCHAR(250), rateAmount VARCHAR(250), ratetype VARCHAR(250), EinvoiceType INT)
                        insert into @tablevar(AgreementId , ProjectId , EinvoiceId , ProjectDescription , DisplayProjectID , CompanyProjectID , ContractProjectPOID , POId , PODescription , DisplayPONumber , PONumber , companypoprojectmatrixid , contractrateid , primaryrateterm , ratedescription , rateAmount , ratetype , EinvoiceType ) exec [dbo].[UspGetProjectPORateDetails_TSAPP] '101898,101899','65767',4


                        SELECT tempTable.DisplayProjectID as ProjectCodeDescription,
                               tempTable.ProjectId as ProjectCodeId,
                               tempTable.rateAmount as RateAmount,
                               tempTable.rateType as RateType,
                               tempTable.ratedescription as RateDescription,
                               tempTable.contractrateid as PayRateId
                        FROM @tablevar tempTable";

                var projectCodeRateDetails = db.Connection.Query<ProjectCodeRateDetails>(query, new
                {
                    contractProjectPOIDList = projectCodeIds,
                    contractRateTermIDList = payRateIds,
                    verticalid = verticalId,

                });

                return projectCodeRateDetails;
            }
        }
    }
}
