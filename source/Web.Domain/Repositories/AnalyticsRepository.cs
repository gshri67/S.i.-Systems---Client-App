﻿using System.Collections;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IAnalyticsRepository
    {
        int TrackUserLoginSuccess(string userLogin);
    }

    public class AnalyticsRepository : IAnalyticsRepository
    {
        private int LogUserLogin(string login, bool success)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[USPSetUserLoginLog]                            
                        @Login, 
                        @Success,
                        @IsAppLogin";

                var result = db.Connection.Query<int>(query, new
                {
                    Login = login,
                    Success = success,
                    IsAppLogin = true

                }).FirstOrDefault();

                return result;
            }
        }

        public int TrackUserLoginSuccess( string userLogin)
        {
            return LogUserLogin(userLogin, success: true);
        }

        public int TrackContractCreatedWithinApp(int agreementId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[USPSetContractIsCreatedInApp_IMSi]                            
                        @AggrementID";

                var result = db.Connection.Query<int>(query, new
                {
                    AgreementID = agreementId

                }).FirstOrDefault();

                return result;
            }
        }
    }
}
