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
    public interface ITimesheetRepository
    {
        IEnumerable<Timesheet> GetTimesheetsForUser(int userId);
        Timesheet SaveTimesheet(Timesheet timesheet, int userId);
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        public IEnumerable<Timesheet> GetTimesheetsForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TOP 6 TimeSheetID AS Id
                            ,Company.CompanyName AS ClientName
                            ,Times.AgreementId as ContractId
                            ,StatusID AS Status
                            ,Periods.TimeSheetAvailablePeriodStartDate AS StartDate
	                        ,Periods.TimeSheetAvailablePeriodEndDate AS EndDate
                            ,Periods.TimeSheetAvailablePeriodID AS AvailableTimePeriodId
                            ,User_Email.PrimaryEmail AS TimeSheetApprover
                        FROM TimeSheet Times
                        LEFT JOIN Agreement ON Agreement.AgreementID = Times.AgreementID
                        LEFT JOIN Company ON Agreement.CompanyID = Company.CompanyID
                        LEFT JOIN TimeSheetAvailablePeriod Periods ON Times.TimeSheetAvailablePeriodID = Periods.TimeSheetAvailablePeriodID
                        LEFT JOIN User_Email ON Times.DirectReportUserId = User_Email.UserID
                        WHERE CandidateUserID = @UserId
                        ORDER BY EndDate DESC, CompanyName";
                
                var timesheets = db.Connection.Query<Timesheet>(query, new { UserId = userId});

                return timesheets;
            }
        }

        public Timesheet SaveTimesheet(Timesheet timesheet, int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_TimesheetTemp_Insert] 
                            @aCandidateUserId
                            ,@aContractID
                            ,@aTSAvailablePeriodID
                            ,@aQuickPay
                            ,@aTSID
                            ,@aTSTempID
                            ,@verticalId";

                //todo: we may need to change this to db.Connection.Execute if the stored procedure doesn't return anything.
                var savedTimesheet = db.Connection.Query<Timesheet>(query, new
                {
                    aCandidateUserId  = userId,
                    aContractID = timesheet.ContractId,
                    aTSAvailablePeriodID = timesheet.AvailableTimePeriodId,
                    aQuickPay = (int?)null,
                    aTSID = (int?)null,
                    aTSTempID = (int?)null,
                    verticalId = 4 //todo: make this not four?
                }).FirstOrDefault();

                return savedTimesheet;
            }
        }
    }
}
