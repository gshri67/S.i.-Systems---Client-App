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
    public interface IDirectReportRepository
    {
        IEnumerable<DirectReport> GetPossibleApproversByAgreementId(int timesheetId);
        DirectReport GetCurrentTimesheetApproverForTimesheet(int timesheetId);
        int UpdateDirectReport(Timesheet timesheet, int previousDirectReportId, int currentUserId);
    }

    public class DirectReportRepository : IDirectReportRepository
    {
        public IEnumerable<DirectReport> GetPossibleApproversByAgreementId(int agreementId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT Users.UserId Id
	                        ,Users.FirstName
	                        ,Users.LastName
	                        ,Email.PrimaryEmail Email
                        FROM Agreement
                        LEFT JOIN Users ON Users.CompanyID = Agreement.CompanyID
                        LEFT JOIN User_Email Email ON Email.UserID = Users.UserID
                        WHERE Agreement.AgreementID = @AgreementId
                        AND Users.UserType = @UserTypeConstant";
                
                var approvers = db.Connection.Query<DirectReport>(query
                    , new { UserTypeConstant = MatchGuideConstants.UserType.ClientContact, AgreementId = agreementId});

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
