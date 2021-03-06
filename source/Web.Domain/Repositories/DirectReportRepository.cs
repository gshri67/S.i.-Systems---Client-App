﻿using System;
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
        DirectReport GetTimesheetApproverByTimesheetId(int timesheetId);
        int UpdateDirectReport(Timesheet timesheet, int previousDirectReportId, int currentUserId);
        IEnumerable<DirectReport> GetTimesheetApproversByCompanyId(int id);
        DirectReport GetTimesheetApproverByAgreementId(int contractId);
        DirectReport GetTimesheetApproverByOpenTimesheetId(int timesheetId);
        int RequestApprovalFromApproverWithId(int timesheetId, int approverId, int currentUserId);
        IEnumerable<int> FrequentyDirectReportIdsByAgreementId(int agreementId);
    }

    public class DirectReportRepository : IDirectReportRepository
    {
        public IEnumerable<DirectReport> GetTimesheetApproversByCompanyId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT Users.UserId Id
	                        ,Users.FirstName
	                        ,Users.LastName
	                        ,Email.PrimaryEmail Email
                        FROM Users 
                        LEFT JOIN User_Email Email ON Email.UserID = Users.UserID
                        WHERE Users.CompanyID = @CompanyId
                        AND Users.UserType IN (SELECT picklistid FROM dbo.udf_getpicklistids('UserRoles','Client Contact',-1))
                        AND Email.PrimaryEmail NOT LIKE '%dummyemail.com'
                        ORDER BY Email";

                var approvers = db.Connection.Query<DirectReport>(query
                    , new { UserTypeConstant = MatchGuideConstants.UserType.ClientContact, CompanyId = id });

                return approvers;
            }
        }

        public IEnumerable<int> FrequentyDirectReportIdsByAgreementId(int agreementId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DISTINCT Users.UserId Id
                            FROM Timesheet
                            LEFT JOIN Users ON Users.UserID = Timesheet.DirectReportUserId
                            LEFT JOIN User_Email Email ON Email.UserID = Users.UserID
                            WHERE timesheet.agreementid = @AgreementId
                            AND UserType IN (SELECT picklistid FROM dbo.udf_getpicklistids('UserRoles','Client Contact',-1))";

                var approverIds = db.Connection.Query<int>(query, new { AgreementId = agreementId });

                return approverIds;
            }
        }

        public DirectReport GetTimesheetApproverByTimesheetId(int timesheetId)
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

        public DirectReport GetTimesheetApproverByAgreementId(int agreementId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT Users.UserID Id
	                        ,Users.FirstName FirstName
	                        ,Users.LastName LastName
	                        ,Email.PrimaryEmail Email
                        FROM Agreement
                        LEFT JOIN Agreement_ContractAdminContactMatrix Matrix 
	                        ON Matrix.AgreementID = Agreement.AgreementID 
	                        AND Matrix.Inactive = 0
                        INNER JOIN Users ON Users.userid = Matrix.DirectReportUserID
                        LEFT JOIN User_Email Email ON Users.UserId = Email.UserID
                        WHERE Agreement.AgreementID = @AgreementId";

                var directReport = db.Connection.Query<DirectReport>(query, new { AgreementId = agreementId }).FirstOrDefault();

                return directReport;
            }
        }

        public DirectReport GetTimesheetApproverByOpenTimesheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT Users.UserID Id
	                        ,Users.FirstName FirstName
	                        ,Users.LastName LastName
	                        ,Email.PrimaryEmail Email
                        FROM TimeSheetTemp Temp 
                        LEFT JOIN Users ON Users.UserID = Temp.ApproverRejectorUserID
                        LEFT JOIN User_Email Email ON Users.UserID = Email.UserID
                        WHERE Temp.TimeSheetTempID = @TimesheetTempId ";

                var directReport = db.Connection.Query<DirectReport>(query, new { TimesheetTempId = timesheetId }).FirstOrDefault();

                return directReport;
            }
        }

        public int RequestApprovalFromApproverWithId(int timesheetId, int approverId, int currentUserId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                      
                        EXECUTE @RC = [dbo].[UspTimesheetResendApproval] 
                           @TimeSheetId
                          ,@DirectReportId
                          ,Resend
                          ,@CreatedBy";

                var returnValue = db.Connection.Query<int>(query, new
                {
                    TimeSheetId = timesheetId,
                    DirectReportId = approverId,
                    CreatedBy = currentUserId
                }).FirstOrDefault();

                return returnValue;
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
                    @agreementid = timesheet.AgreementId,
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
