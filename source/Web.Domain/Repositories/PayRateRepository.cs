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
        IEnumerable<int> GetProjectIdFromTimesheet(Timesheet timesheet);
        IEnumerable<int> GetContractRateIdFromOpenTimesheet(Timesheet timesheet);
        IEnumerable<int> GetContractRateIdFromSavedOrSubmittedTimesheet(Timesheet timesheet);
        IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(IEnumerable<int> projectCodeIds, IEnumerable<int> payRateIds);
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


        public IEnumerable<int> GetContractRateIdFromOpenTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"Declare @tablevar table(ContractRateID INT, PrimaryRateTerm BIT, RateDescription VARCHAR(250), HoursPerDay DECIMAL, BillRate MONEY, PayRate MONEY)
                        insert into @tablevar(ContractRateID, PrimaryRateTerm, RateDescription, HoursPerDay, BillRate, PayRate) EXECUTE [dbo].[UspGetRateTermDetails_TSAPP] 
                           @AgreementId
                          ,@TimesheetID
                          ,@Startdate
                          ,@Enddate
                          ,@TimesheetStatus

                        SELECT tempTable.ContractRateID as ContractID,
                               tempTable.PrimaryRateTerm as PrimaryRateTerm,
                               tempTable.RateDescription as RateDescription,
                               tempTable.HoursPerDay as HoursPerDay,
                               tempTable.BillRate as BillRate,
                               tempTable.PayRate as PayRate

                        FROM @tablevar tempTable";

                var rateId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.ContractId,
                    TimesheetID = timesheet.Id,
                    Startdate = timesheet.StartDate,
                    Enddate = timesheet.EndDate,
                    TimesheetStatus = string.Empty
                });

                return rateId;
            }
        }

        public IEnumerable<int> GetContractRateIdFromSavedOrSubmittedTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

              const string query =
                    @"Declare @tablevar table(ContractProjectPOId INT, ContractRateID INT, ProjectPORateList INT, PrimaryRateTerm BIT, RateDescription VARCHAR(250), HoursPerDay DECIMAL, BillRate MONEY, PayRate MONEY)
                        insert into @tablevar(ContractProjectPOId, ContractRateID, ProjectPORateList, PrimaryRateTerm, RateDescription, HoursPerDay, BillRate, PayRate) EXECUTE [dbo].[UspGetRateTermDetails_TSAPP] 
                           @AgreementId
                          ,@TimesheetID
                          ,@Startdate
                          ,@Enddate
                          ,@TimesheetStatus

                        SELECT  tempTable.ContractProjectPOId as ContractProjectPOId,
		                        tempTable.ContractRateID as ContractID,
		                        tempTable.ProjectPORateList as ProjectPORateList,
                                tempTable.PrimaryRateTerm as PrimaryRateTerm,
                                tempTable.RateDescription as RateDescription,
                                tempTable.HoursPerDay as HoursPerDay,
                                tempTable.BillRate as BillRate,
                                tempTable.PayRate as PayRate

                        FROM @tablevar tempTable";

                string status;

                if (timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted)
                    status = "Submitted";
                else
                    status = "Saved";

                var rateId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.ContractId,
                    TimesheetID = timesheet.Id,
                    Startdate = timesheet.StartDate,
                    Enddate = timesheet.EndDate,
                    TimesheetStatus = status
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

        private static string ToCommaSeperatedString(IEnumerable<int> myEnum)
        {
            return string.Join(",", myEnum);
        }

        public IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(IEnumerable<int> projectCodeIds, IEnumerable<int> payRateIds)
        {
            var commaSeperatedProjectCodes = ToCommaSeperatedString(projectCodeIds);
            var commaSeperatedPayRates = ToCommaSeperatedString(payRateIds);

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
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
                    contractProjectPOIDList = commaSeperatedProjectCodes,
                    contractRateTermIDList = commaSeperatedPayRates,
                    verticalid = MatchGuideConstants.VerticalId.IT
                });

                return projectCodeRateDetails;
            }
        }
    }
}
