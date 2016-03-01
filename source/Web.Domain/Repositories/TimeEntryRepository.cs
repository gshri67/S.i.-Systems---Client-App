using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface ITimeEntryRepository
    {
        IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheetId);
        void SaveTimeEntry(int id, TimeEntry entry);
        int SubmitTimeEntry(int timesheetId, TimeEntry timeEntry);
    }

    public class TimeEntryRepository : ITimeEntryRepository
    {
        public IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheet)
        {
            if (timesheet.Id != 0) 
                return GetSubmittedTimesheetEntries(timesheet);
            
            if (timesheet.OpenStatusId != 0)
                return GetSavedTimesheetEntries(timesheet);

            return Enumerable.Empty<TimeEntry>();
        }

        private IEnumerable<TimeEntry> GetSavedTimesheetEntries(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DetTemp.TimeSheetDetailTempID AS Id
	                        ,CAST(DetTemp.UnitValue AS FLOAT) AS Hours
	                        ,DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), DetTemp.Day, 0, 0, 0, 0) AS Date
	                        ,DetTemp.ProjectID AS ProjectId
	                        ,DetTemp.PONumber
	                        ,DetTemp.ContractProjectPoID
	                        ,DetTemp.ContractRateID
	                        ,DetTemp.Description AS PODescription
	                        ,DetTemp.InvoiceCodeId
	                        ,DetTemp.verticalid
                            ,ContractDetail.RateDescription AS ratedescription
	                        ,CONVERT(VARCHAR(30), ContractDetail.PayRate)  AS rateamount
                        FROM TimeSheetDetailTemp DetTemp
                        LEFT JOIN TimeSheetTemp TSTemp ON TSTemp.TimeSheetTempID = DetTemp.TimesheetTempID
                        LEFT JOIN TimeSheetAvailablePeriod Period ON Period.TimeSheetAvailablePeriodID = TSTemp.TimeSheetAvailablePeriodID
                        LEFT JOIN Agreement_ContractRateDetail ContractDetail ON ContractDetail.ContractRateID = DetTemp.ContractRateID
                        WHERE DetTemp.TimeSheetTempID = @TimesheetTempId
                        AND DetTemp.Inactive = 0";

                var timeEntries = db.Connection.Query<TimeEntry, ProjectCodeRateDetails, TimeEntry>(query,
                    (timeEntry, codeRate) =>
                    {
                        timeEntry.CodeRate = codeRate;
                        return timeEntry;
                    },
                    splitOn: "ProjectId",
                    param: new { TimesheetTempId = timesheet.OpenStatusId });
                
                return timeEntries;
            }
        }

        private IEnumerable<TimeEntry> GetSubmittedTimesheetEntries(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                //note that there is a plus one on the date because dates seem to be zero indexed.
                const string query =
                        @"SELECT Details.TimeSheetDetailID AS Id
                            ,CAST(Details.UnitValue AS FLOAT) AS Hours
                            ,DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), Details.Day + 1, 0, 0, 0, 0) AS Date
                            ,Details.ProjectID AS ProjectId
                            ,Details.PONumber
                            ,Details.ContractProjectPoID
                            ,Details.ContractRateID
                            ,Details.Description AS PODescription
                            ,Details.InvoiceCodeId
                            ,Details.verticalid
                            ,ContractDetail.RateDescription AS ratedescription
	                        ,CONVERT(VARCHAR(30), ContractDetail.PayRate)  AS rateamount
                        FROM TimeSheetDetail Details
                        LEFT JOIN TimeSheet ON TimeSheet.TimeSheetID = Details.TimesheetID
                        LEFT JOIN TimeSheetAvailablePeriod Period ON Period.TimeSheetAvailablePeriodID = TimeSheet.TimeSheetAvailablePeriodID
                        LEFT JOIN Agreement_ContractRateDetail ContractDetail ON ContractDetail.ContractRateID = Details.ContractRateID
                        WHERE Details.TimeSheetID = @TimesheetId";

                var timeEntries = db.Connection.Query<TimeEntry, ProjectCodeRateDetails, TimeEntry>(query,
                    (timeEntry, codeRate) =>
                    {
                        timeEntry.CodeRate = codeRate;
                        return timeEntry;
                    },
                    splitOn: "ProjectId",
                    param: new { TimesheetId = timesheet.Id });

                return timeEntries;
            }
        }

        public void SaveTimeEntry(int id, TimeEntry entry)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"EXECUTE [dbo].[sp_TimesheetTempDetail_Insert] 
                           @aContractrateid
                          ,@aPoNumber
                          ,@aProjectId
                          ,@acontractprojectpoid
                          ,@aDay
                          ,@aUnitValue
                          ,@aGeneralProjPODesc
                          ,@aTimesheetTempId";

                //note that there is a -1 on day because days seem to be zero indexed in MG
                db.Connection.Execute(query, new
                {
                    aContractrateid = entry.CodeRate.contractrateid,
                    aPoNumber = entry.CodeRate.PONumber,
                    aProjectID = entry.CodeRate.ProjectId, 
                    acontractprojectpoid = entry.CodeRate.ContractProjectPOID, 
                    aDay = entry.Date.Day - 1,
                    aUnitValue = entry.Hours,
                    aGeneralProjPODesc = entry.CodeRate.PODescription, 
                    aTimesheetTempID = id,
                }, commandType:CommandType.StoredProcedure);
            }
        }

//        public PayRate GetPayRateById(TimeEntry entry)
//        {
//            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
//            {
//                const string query =
//                        @"SELECT rateDetail.ContractRateId as Id
//	                        ,RateDescription AS RateDescription
//	                        ,PayRate AS Rate	
//                        from TimeSheetDetail dets
//                        left join Agreement_ContractRateDetail rateDetail on rateDetail.ContractRateID = dets.ContractRateID
//                        where TimeSheetDetailID = @TimeEntryId";

//                var payRate = db.Connection.Query<PayRate>(query, new { TimeEntryId = entry.Id }).SingleOrDefault();

//                return payRate;
//            }
//        }

        public int SubmitTimeEntry(int timesheetId, TimeEntry timeEntry)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_TimesheetDetail_Insert] 
                           @aContractrateid
                          ,@aPoNumber
                          ,@aProjectId
                          ,@acontractprojectpoid
                          ,@aDay
                          ,@aUnitValue
                          ,@aGeneralProjPODesc
                          ,@aTimesheetId
                          ,@verticalId
                          ,@InvoiceCodeId";

                var submittedTimesheetId = db.Connection.Query<int>(query, new
                {
                     aContractrateid = timeEntry.CodeRate.contractrateid,
                     aPoNumber  = timeEntry.CodeRate.PONumber,
                     aProjectId = timeEntry.CodeRate.ProjectId,
                     acontractprojectpoid = timeEntry.CodeRate.ContractProjectPOID,
                     aDay = timeEntry.Date.Day,
                     aUnitValue = timeEntry.Hours,
                     aGeneralProjPODesc  = timeEntry.CodeRate.PODescription,
                     aTimesheetId = timesheetId,
                     verticalId = MatchGuideConstants.VerticalId.IT,
                     InvoiceCodeId = timeEntry.CodeRate.EinvoiceId
                }).FirstOrDefault();

                return submittedTimesheetId;
            }
        }
    }
}
