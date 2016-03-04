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
        //IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheetId);
        int SaveTimeEntry(int id, TimeEntry entry);
        int SubmitTimeEntry(int timesheetId, TimeEntry timeEntry);
        IEnumerable<TimeEntry> GetEntriesForNonOpenTimesheet(Timesheet timesheet);
        IEnumerable<TimeEntry> GetEntriesForOpenTimesheet(Timesheet timesheet);
        IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheet);
    }

    public class TimeEntryRepository : ITimeEntryRepository
    {

        public IEnumerable<TimeEntry> GetEntriesForOpenTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DetTemp.TimeSheetDetailTempID AS Id
	                        ,CAST(DetTemp.UnitValue AS FLOAT) AS Hours
	                        ,DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), DetTemp.Day, 0, 0, 0, 0) AS EntryDate
                            ,'' as split
	                        ,DetTemp.PONumber
	                        ,DetTemp.ProjectID AS ProjectId
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
                        AND DetTemp.Inactive = 0
                        AND DetTemp.Day != 0";

                var timeEntries = db.Connection.Query<TimeEntry, ProjectCodeRateDetails, TimeEntry>(query,
                    (timeEntry, codeRate) =>
                    {
                        timeEntry.CodeRate = codeRate;
                        return timeEntry;
                    },
                    splitOn: "split",
                    param: new { TimesheetTempId = timesheet.OpenStatusId });
                
                return timeEntries;
            }
        }

        private static bool ShouldLoadEntriesFromTimesheetId(Timesheet timesheet)
        {
            return timesheet.Status == MatchGuideConstants.TimesheetStatus.Open
                && (timesheet.Id == 0 || timesheet.OpenStatusId != 0);
        }

        public IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheet)
        {
            return ShouldLoadEntriesFromTimesheetId(timesheet)
                ? GetEntriesForOpenTimesheet(timesheet)
                : GetEntriesForNonOpenTimesheet(timesheet);
        }

        public IEnumerable<TimeEntry> GetEntriesForNonOpenTimesheet(Timesheet timesheet)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                //note that there is a plus one on the date because dates seem to be zero indexed.
                const string query =
                        @"SELECT Details.TimeSheetDetailID AS Id
                            ,CAST(Details.UnitValue AS FLOAT) AS Hours
                            ,DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), Details.Day, 0, 0, 0, 0) as EntryDate
                            ,'' as split
                            ,Details.PONumber
                            ,Details.ProjectID AS ProjectId
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
                        WHERE Details.TimeSheetID = @TimesheetId
                        AND Details.Day != 0";

                var timeEntries = db.Connection.Query<TimeEntry, ProjectCodeRateDetails, TimeEntry>(query,
                    (timeEntry, codeRate) =>
                    {
                        timeEntry.CodeRate = codeRate;
                        return timeEntry;
                    },
                    splitOn: "split",
                    param: new { TimesheetId = timesheet.Id });

                return timeEntries;
            }
        }

        public int SaveTimeEntry(int id, TimeEntry entry)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"INSERT INTO [dbo].[TimeSheetDetailTemp]
                            ([ContractRateID]
                            ,[PONumber]
                            ,[ProjectID]
                            ,[ContractProjectPoID]
                            ,[Day]
                            ,[UnitValue]
                            ,[Description]
                            ,[TimesheetTempID]
                            ,[Inactive]
                            ,[verticalid]
                            ,[InvoiceCodeId])
                        VALUES
                            (@ContractRateId
                            ,@PONumber
                            ,@ProjectId
                            ,@ContractProjectPoID
                            ,@Day
                            ,@UnitValue
                            ,@Description
                            ,@TimesheetTempID
                            ,@Inactive
                            ,@verticalid
                            ,@InvoiceCodeId)
                        select @@identity as Id";

                var insertedId = db.Connection.Query<int>(query, new
                {
                    ContractRateId = entry.CodeRate.contractrateid,
                    PONumber = entry.CodeRate.PONumber,
                    ProjectID = entry.CodeRate.ProjectId, 
                    ContractProjectPoID = entry.CodeRate.ContractProjectPOID, 
                    Day = entry.EntryDate.Day,
                    UnitValue = entry.Hours,
                    Description = entry.CodeRate.PODescription, 
                    TimesheetTempID = id,
                    Inactive = 0,
                    verticalid = MatchGuideConstants.VerticalId.IT,
                    InvoiceCodeId = entry.CodeRate.EinvoiceId
                }).FirstOrDefault();

                return insertedId;
            }
        }

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
                     aDay = timeEntry.EntryDate.Day,
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
