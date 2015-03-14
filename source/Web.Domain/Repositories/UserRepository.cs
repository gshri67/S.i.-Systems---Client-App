using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IUserRepository
    {
        User Find(int id);

        User FindByName(string username);
    }

    public class UserRepository : IUserRepository
    {
        const string UserQueryBase = "SELECT U.UserId Id, U.CompanyID ClientId, C.CompanyName, "
                               + "U.FirstName FirstName, U.LastName LastName, U.ClientPortalTypeID ClientPortalType, "
                               + "U.ClientPortalFTAlumniTypeID FloThruAlumniAccess, UL.Login Login, UL.Password PasswordHash, U.UserType, MaxVisibleRatePerHour as ClientsMaxVisibleRate "
                               + "FROM [User_Login] as UL, [Users] as U, [Company] as C "
                               + "WHERE U.UserID=UL.UserID";

        private readonly IClientDetailsRepository _detailsRepository;

        public UserRepository(IClientDetailsRepository detailsRepository)
        {
            this._detailsRepository = detailsRepository;
        }

        public User Find(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + " AND U.UserId = @Id";

                var user = db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();

                // Temporary work around to get list of participating 
                // companies until the matchguide database is updated
                if (user != null && user.UserType == MatchGuideConstants.UserType.ClientContact && Settings.ShouldUseConfiguredParticipatingCompaniesList)
                {
                    var clientDetails = this._detailsRepository.GetClientDetails(user.ClientId);
                    user.FloThruAlumniAccess = clientDetails.HasAccess ? MatchGuideConstants.FloThruAlumniAccess.AllAccess : MatchGuideConstants.FloThruAlumniAccess.NoAccess;
                }
                return user;
            }
        }

        public User FindByName(string username)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + " AND UL.Login = @Username";

                var user = db.Connection.Query<User>(query, new {Username = username}).FirstOrDefault();

                // Temporary work around to get list of participating 
                // companies until the matchguide database is updated
                if (user != null && user.UserType == MatchGuideConstants.UserType.ClientContact && Settings.ShouldUseConfiguredParticipatingCompaniesList)
                {
                    var clientDetails = this._detailsRepository.GetClientDetails(user.ClientId);
                    user.FloThruAlumniAccess = clientDetails.HasAccess ? MatchGuideConstants.FloThruAlumniAccess.AllAccess : MatchGuideConstants.FloThruAlumniAccess.NoAccess;
                }
                return user;
            }
        }
    }
}
