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

        /// <summary>
        /// Saves the timesheet into the temporary table, returning the ID of the row created.
        /// </summary>
        /// <param name="timesheet"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int SaveTimesheet(Timesheet timesheet, int userId);

        /// <summary>
        /// Submits the Timesheet as vacation for the user, automatically approving, and returning the ID of the new record.
        /// </summary>
        /// <param name="timesheet"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int SubmitZeroTimeForUser(Timesheet timesheet, int userId);
        Timesheet GetTimesheetsById(int timesheetId);
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        public Timesheet GetTimesheetsById(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TimeSheetID AS Id
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
                        WHERE Times.TimeSheetId = @TimesheetId
                        ORDER BY EndDate DESC, CompanyName";

                var timesheet = db.Connection.Query<Timesheet>(query, new { TimesheetId = timesheetId }).FirstOrDefault();

                return timesheet;
            }
        }

        public IEnumerable<Timesheet> GetTimesheetsForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TimeSheetID AS Id
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
                        AND ResubmittedToID IS NULL
                        ORDER BY EndDate DESC, CompanyName";
                
                var timesheets = db.Connection.Query<Timesheet>(query, new { UserId = userId});

                return timesheets;
            }
        }

        public int SaveTimesheet(Timesheet timesheet, int userId)
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

                var savedTimesheetTempId = db.Connection.Query<int>(query, new
                {
                    aCandidateUserId  = userId,
                    aContractID = timesheet.ContractId,
                    aTSAvailablePeriodID = timesheet.AvailableTimePeriodId,
                    aQuickPay = (int?)null,
                    aTSID = IntegerOrNullIfZero(timesheet.Id),
                    aTSTempID = IntegerOrNullIfZero(timesheet.OpenStatusId),
                    verticalId = 4 //todo: make this not four?
                }).FirstOrDefault();

                return savedTimesheetTempId;
            }
        }

        public int SubmitZeroTimeForUser(Timesheet timesheet, int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @newTSID INT
                        DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Timesheet_Update] 
                           @aTimesheetId
                          ,@aCandidateUserId
                          ,@aContractID
                          ,@aTimesheetavailableperiodid
                          ,@aTSSubmittedName
                          ,@verticalid
                          ,@aTimesheetType
                          ,@TSstatus
                     SET @newTSID =  @@identity                  
                     SELECT @newTSID as TimesheetId";

                var savedTimesheetTempId = db.Connection.Query<int>(query, new
                {
                    aTimesheetId = IntegerOrNullIfZero(timesheet.Id),
                    aCandidateUserId = userId,
                    aContractID = timesheet.ContractId,
                    aTimesheetavailableperiodid = timesheet.AvailableTimePeriodId,
                    aTSSubmittedName = (string)null, //Name of the Submitted PDF
                    verticalId = MatchGuideConstants.VerticalId.IT,
                    aTimesheetType = MatchGuideConstants.TimesheetType.ETimesheet.ToString(),
                    TSstatus = MatchGuideConstants.TimesheetStatus.Approved.ToString()
                }).FirstOrDefault();

                return savedTimesheetTempId;
            }
        }

        private static int? IntegerOrNullIfZero(int number)
        {
            return number == 0 ? (int?)null : number;
        }
    }
}
