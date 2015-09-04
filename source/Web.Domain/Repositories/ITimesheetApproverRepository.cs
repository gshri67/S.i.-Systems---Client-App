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
        IEnumerable<string> GetApproversForUser(int userId);
    }

    public class TimesheetApproverRepository : ITimesheetApproverRepository
    {
        public IEnumerable<string> GetApproversForUser(int clientId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
//                const string query = 
//                        @"SELECT TOP 6 TimeSheetID AS Id
//                            ,Company.CompanyName AS ClientName
//                            ,StatusID AS Status
//                            ,Periods.TimeSheetAvailablePeriodStartDate AS StartDate
//	                        ,Periods.TimeSheetAvailablePeriodEndDate AS EndDate
//                            ,User_Email.PrimaryEmail AS TimeSheetApprover
//                        FROM TimeSheet Times
//                        LEFT JOIN Agreement ON Agreement.AgreementID = Times.AgreementID
//                        LEFT JOIN Company ON Agreement.CompanyID = Company.CompanyID
//                        LEFT JOIN TimeSheetAvailablePeriod Periods ON Times.TimeSheetAvailablePeriodID = Periods.TimeSheetAvailablePeriodID
//                        LEFT JOIN User_Email ON Times.DirectReportUserId = User_Email.UserID
//                        WHERE CandidateUserID = @UserId
//                        ORDER BY EndDate DESC, CompanyName";
                
//                var timesheets = db.Connection.Query<Timesheet>(query, new { UserId = userId});
                var approvers = new List<string>() { "fred.flinstone@email.com", "greg.flinstone@email.com", "bambam.flinstone@email.com" };
                return approvers;
            }
        }
    }
}
