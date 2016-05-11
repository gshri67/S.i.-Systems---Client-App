using System.Linq;
using Dapper;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IAnalyticsRepository
    {
        int TrackUserLogin( int userId, bool loginSuccessful );
    }

    public class AnalyticsRepository : IAnalyticsRepository
    {
        public int TrackUserLogin( int userId, bool loginSuccessful )
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
                    Login = userId,
                    Success = loginSuccessful,
                    IsAppLogin = true

                }).FirstOrDefault();

                return result;
            }
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
