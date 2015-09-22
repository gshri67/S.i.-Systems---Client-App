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
                        @"SELECT TD.TimesheetDetailId, AC.RateDescription  as PayRate,       
                        (ISNULL(P.ProjectId,'')  
                        +     
                        CASE WHEN LEN(ISNULL(P.ProjectId,'') )>0 AND LEN(ISNULL(PONumber,''))>0 THEN '/' ELSE '' END + ISNULL(PONumber,'')   
                        + 
                        IsNull(I.InvoiceCodeText,''))  AS ProjectCode,    
                        DATETIMEFROMPARTS(YEAR(Period.TimeSheetAvailablePeriodStartDate), MONTH(Period.TimeSheetAvailablePeriodStartDate), day, 0, 0, 0, 0) AS Date,    
                        CAST(UnitValue AS FLOAT) AS Hours
                        FROM Timesheet t     
                        INNER JOIN Timesheetdetail TD ON T.TimesheetId = TD.TimesheetId     
                        INNER JOIN Agreement_ContractRatedetail AC ON TD.contractRateId = AC.ContractRateId    
                        LEFT JOIN CompanyProject P ON P.CompanyProjectId= TD.ProjectId    
                        LEFT JOIN ContractInvoiceCode I on I.invoicecodeid=TD.invoicecodeid AND I.contractid=T.Agreementid     
                        LEFT JOIN TimeSheetAvailablePeriod Period ON T.TimeSheetAvailablePeriodID = Period.TimeSheetAvailablePeriodID
                        WHERE T.TimesheetId = @TimesheetId";
                
                var timeEntries = db.Connection.Query<TimeEntry>(query, new { TimesheetId = timesheetId});

                return timeEntries;
            }
        }
    }
}
