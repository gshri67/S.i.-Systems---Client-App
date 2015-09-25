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
        void SaveTimeEntry(int id, TimeEntry entry);
        PayRate GetPayRateById(TimeEntry entry);
    }

    public class TimeEntryRepository : ITimeEntryRepository
    {
        public IEnumerable<TimeEntry> GetTimeEntriesByTimesheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TD.TimesheetDetailId AS Id,
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

        public void SaveTimeEntry(int id, TimeEntry entry)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_TimesheetTempDetail_Insert] 
                           @aContractrateid
                          ,@aPoNumber
                          ,@aProjectId
                          ,@acontractprojectpoid
                          ,@aDay
                          ,@aUnitValue
                          ,@aGeneralProjPODesc
                          ,@aTimesheetTempId";

                //todo: we may need to change this to db.Connection.Execute if the stored procedure doesn't return anything.
                db.Connection.Query<TimeEntry>(query, new
                {
                    //todo: commented values are to be retrieved from the query given to us from Chennai (This is being put in a stored procedure).
                   aContractrateid = entry.PayRate.Id,
                   aPoNumber = entry.ProjectCode, //PONumber
                   aProjectID = "", //ProjectId
                   acontractprojectpoid = "", //contractprojectpoid
                   aDay = entry.Date.Day,
                   aUnitValue = entry.Hours,
                   aGeneralProjPODesc = "", //PODescription?
                   aTimesheetTempID = id,
                });
            }
        }

        public PayRate GetPayRateById(TimeEntry entry)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT rateDetail.ContractRateId as Id
	                        ,RateDescription AS RateDescription
	                        ,PayRate AS Rate	
                        from TimeSheetDetail dets
                        left join Agreement_ContractRateDetail rateDetail on rateDetail.ContractRateID = dets.ContractRateID
                        where TimeSheetDetailID = @TimeEntryId";

                var payRate = db.Connection.Query<PayRate>(query, new { TimeEntryId = entry.Id }).SingleOrDefault();

                return payRate;
            }
        }
    }
}
