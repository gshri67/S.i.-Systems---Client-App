using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IUserRepository
    {
        User Find(int id);

        User FindByName(string username);
    }

    public class UserRepository : IUserRepository
    {
        const string UserQueryBase = @"SELECT U.[UserId] Id, U.[FirstName] FirstName, U.[LastName] LastName, 
	                                    U.[UserType], U.[ClientPortalTypeID] ClientPortalType, U.ClientPortalFTAlumniTypeID FloThruAlumniAccess,
	                                    UL.[Login], UL.[Password] PasswordHash,
	                                    C.[CompanyID] CompanyId, C.[CompanyName],
	                                    C.[MaxVisibleRatePerHour] as ClientsMaxVisibleRate, C.[IsHavingFTAlumni] IsCompanyParticipating
                                    FROM [Users] as U, [User_Login] as UL, [Company] as C
                                    WHERE U.UserID=UL.UserID AND U.CompanyID = C.CompanyID";

        public User Find(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + " AND U.UserId = @Id";

                var user = db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();
                SetAccessLevel(user);
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
                SetAccessLevel(user);
                return user;
            }
        }

        #region Workaround

        private readonly ICompanyRepository _companyRepository;

        public UserRepository(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        /// <summary>
        /// Temporary workaround to set the user and company access
        /// permissions until we have real data
        /// </summary>
        /// <param name="user">The client user</param>
        private void SetAccessLevel(User user)
        {
            if (Settings.ShouldUseConfiguredParticipatingCompaniesList 
                && user.UserType == MatchGuideConstants.UserType.ClientContact)
            {
                var associatedCompanies = this._companyRepository.GetAllAssociatedCompanyIds(user.CompanyId);
                user.IsCompanyParticipating = Settings.ParticipatingCompaniesList.Values.Any(v => associatedCompanies.Contains(v));
                user.FloThruAlumniAccess = MatchGuideConstants.FloThruAlumniAccess.AllAccess;
            }
        }

        #endregion
    }
}
