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
    }
}
