using System;
using System.Collections.Generic;
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
        IEnumerable<TimeEntry> GetTimeEntriesByTimesheetId(int timesheetId);
    }

    public class TimeEntryRepository : ITimeEntryRepository
    {
        public IEnumerable<TimeEntry> GetTimeEntriesByTimesheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TimeSheetDetailID
	                          ,Company.CompanyName AS ClientName
                              ,RateDetail.PayRate
                              ,PONumber AS ProjectCode
	                          ,DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), day, 0, 0, 0, 0) AS Date
                              ,CAST(UnitValue AS FLOAT) AS Hours
                          FROM TimeSheetDetail Dets
                          LEFT JOIN Agreement_ContractRateDetail RateDetail ON RateDetail.ContractRateID = Dets.ContractRateID
                          LEFT JOIN TimeSheet ON Dets.TimesheetID = TimeSheet.TimeSheetID  
                          LEFT JOIN Agreement ON Agreement.AgreementID = TimeSheet.AgreementID
                          LEFT JOIN Company ON Agreement.CompanyID = Company.CompanyID
                          LEFT JOIN TimeSheetAvailablePeriod Period ON TimeSheet.TimeSheetAvailablePeriodID = Period.TimeSheetAvailablePeriodID
                          WHERE Dets.TimesheetID = @TimesheetId";
                
                var timeEntries = db.Connection.Query<TimeEntry>(query, new { TimesheetId = timesheetId});

                return timeEntries;
            }
        }
    }
}
