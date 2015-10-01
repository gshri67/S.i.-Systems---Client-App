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
    public interface ITimesheetApproverRepository
    {
        IEnumerable<string> GetPossibleApproversByTimesheetId(int timesheetId);
        DirectReport GetCurrentTimesheetApproverForTimesheet(int timesheetId);
        int UpdateDirectReport(Timesheet timesheet, int previousDirectReportId, int currentUserId);
    }

    public class TimesheetApproverRepository : ITimesheetApproverRepository
    {
        public IEnumerable<string> GetPossibleApproversByTimesheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT UE.PrimaryEmail
                            FROM Users U
                            LEFT JOIN User_Email UE ON UE.UserID = U.UserID
                            LEFT JOIN Timesheet ON Timesheet.TimesheetId = @TimesheetId
                            LEFT JOIN Agreement ON Timesheet.AgreementId = Agreement.AgreementId
                            WHERE UserType = @UserTypeConstant
                            AND U.CompanyID = Agreement.CompanyID";
                
                var approvers = db.Connection.Query<string>(query
                    , new { UserTypeConstant = MatchGuideConstants.UserType.ClientContact, TimesheetId = timesheetId });

                return approvers;
            }
        }

        public DirectReport GetCurrentTimesheetApproverForTimesheet(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DirectReport.UserID Id
	                            ,DirectReport.FirstName FirstName
	                            ,DirectReport.LastName LastName
	                            ,DirectReportEmail.PrimaryEmail Email
                            FROM TimeSheet Times
                            LEFT JOIN Users DirectReport ON Times.DirectReportUserId = DirectReport.UserID
                            LEFT JOIN User_Email DirectReportEmail ON Times.DirectReportUserId = DirectReportEmail.UserID
                            WHERE Times.TimeSheetId = @TimesheetId";

                var directReport = db.Connection.Query<DirectReport>(query, new { TimesheetId = timesheetId }).FirstOrDefault();

                return directReport;
            }
        }

        public int UpdateDirectReport(Timesheet timesheet, int previousDirectReportId, int currentUserId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Timesheet_ManageDirectReports_update] 
                           @agreementid
                          ,@olddirectreportuserid
                          ,@newdirectreportuserid
                          ,@updateuserid
                          ,@inactive";

                var submittedTimesheetId = db.Connection.Query<int>(query, new
                {
                    @agreementid = timesheet.ContractId,
                    @olddirectreportuserid = previousDirectReportId,
                    @newdirectreportuserid = timesheet.TimesheetApprover.Id,
                    @updateuserid = currentUserId,
                    @inactive = (bool?)null //this value is unused by the stored procedure
                }).FirstOrDefault();

                return submittedTimesheetId;
            }
        }
    }
}
