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
        IEnumerable<int> GetProjectIdsFromTimesheet(Timesheet timesheet);
        IEnumerable<int> GetContractRateIdFromOpenTimesheet(Timesheet timesheet);
        IEnumerable<int> GetContractRateIdFromSavedOrSubmittedTimesheet(Timesheet timesheet);
        IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(IEnumerable<int> projectCodeIds, IEnumerable<int> payRateIds);
    }

    public class PayRateRepository : IPayRateRepository
    {
        public IEnumerable<int> GetContractRateIdFromOpenTimesheet(Timesheet timesheet)
        {
            //note that we need to do this to account for contracts that start or end in the middle of a pay period.
            var startDate = LaterDate(timesheet.StartDate, timesheet.AgreementStartDate);
            var endDate = EarlierDate(timesheet.EndDate, timesheet.AgreementEndDate);

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"Declare @tablevar table(ContractRateID INT, PrimaryRateTerm BIT, RateDescription VARCHAR(250), HoursPerDay DECIMAL, BillRate MONEY, PayRate MONEY)
                        insert into @tablevar(ContractRateID, PrimaryRateTerm, RateDescription, HoursPerDay, BillRate, PayRate) EXECUTE [dbo].[UspGetRateTermDetails_TSAPP] 
                           @AgreementId
                          ,null
                          ,@Startdate
                          ,@Enddate

                        SELECT tempTable.ContractRateID as ContractID

                        FROM @tablevar tempTable";

                var rateId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.AgreementId,
                    Startdate = startDate,
                    Enddate = endDate
                });

                return rateId;
            }
        }

        public IEnumerable<int> GetContractRateIdFromSavedOrSubmittedTimesheet(Timesheet timesheet)
        {
            var status = timesheet.Status == MatchGuideConstants.TimesheetStatus.Open ? "Saved" : "Submitted";

            //note that we need to do this to account for contracts that start or end in the middle of a pay period.
            var startDate = LaterDate(timesheet.StartDate, timesheet.AgreementStartDate);
            var endDate = EarlierDate(timesheet.EndDate, timesheet.AgreementEndDate);

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

              const string query =
                    @"Declare @tablevar table(ContractProjectPOId INT, ContractRateID INT, ProjectPORateList VARCHAR(255), PrimaryRateTerm BIT, RateDescription VARCHAR(250), HoursPerDay DECIMAL, BillRate MONEY, PayRate MONEY)
                        insert into @tablevar(ContractProjectPOId, ContractRateID, ProjectPORateList, PrimaryRateTerm, RateDescription, HoursPerDay, BillRate, PayRate) EXECUTE [dbo].[UspGetRateTermDetails_TSAPP] 
                           @AgreementId
                          ,@TimesheetID
                          ,@Startdate
                          ,@Enddate
                          ,@TimesheetStatus

                        SELECT  tempTable.ContractRateID as ContractID

                        FROM @tablevar tempTable";
                
                var rateId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.AgreementId,
                    TimesheetID = timesheet.OpenStatusId,
                    Startdate = startDate,
                    Enddate = endDate,
                    TimesheetStatus = new DbString
                    {
                        Value = status,
                        Length = 10
                    }
                });

                return rateId;
            }
        }

        public IEnumerable<int> GetProjectIdsFromTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"Declare @tablevar2 table(AgreementId INT, DisplayProjectPOEInvoiceID VARCHAR(255), Valprojectpoid INT, IsGeneralProjectPO BIT, EInvoiceType BIT)
                    insert into @tablevar2(AgreementId , DisplayProjectPOEInvoiceID , Valprojectpoid , IsGeneralProjectPO , EInvoiceType ) EXECUTE [dbo].[UspGetProjectPODetails_TSAPP] @AgreementId

                    SELECT Valprojectpoid FROM @tablevar2";

                var projectId = db.Connection.Query<int>(query, new
                {
                    AgreementId = timesheet.AgreementId
                });

                return projectId;
            }
        }

        public IEnumerable<ProjectCodeRateDetails> GetProjectCodesAndPayRatesFromIds(IEnumerable<int> projectCodeIds, IEnumerable<int> payRateIds)
        {
            var commaSeperatedProjectCodes = ToCommaSeperatedString(projectCodeIds);
            var commaSeperatedPayRates = ToCommaSeperatedString(payRateIds);

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"EXEC [dbo].[UspGetProjectPORateDetails_TSAPP] @contractProjectPOIDList, @contractRateTermIDList, @verticalid";

                var projectCodeRateDetails = db.Connection.Query<ProjectCodeRateDetails>(query, new
                {
                    contractProjectPOIDList = new DbString
                    {
                        Value = commaSeperatedProjectCodes,
                        Length = 1000
                    },
                    contractRateTermIDList = new DbString
                    {
                        Value = commaSeperatedPayRates,
                        Length = 1000
                    },
                    verticalid = MatchGuideConstants.VerticalId.IT
                });

                return projectCodeRateDetails;
            }
        }

        #region HelperFunctions
        private static string ToCommaSeperatedString(IEnumerable<int> myEnum)
        {
            return string.Join(",", myEnum);
        }

        private static DateTime LaterDate(DateTime first, DateTime second)
        {
            return first > second ? first : second;
        }

        private static DateTime EarlierDate(DateTime first, DateTime second)
        {
            return first < second ? first : second;
        }
        #endregion
    }
}
