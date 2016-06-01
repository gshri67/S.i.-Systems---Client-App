using System.Linq;
using Dapper;
using System.Threading.Tasks;
using SiSystems.SharedModels;

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
                                        FROM [Users] as U
                                        JOIN [User_Login] as UL on U.UserID = UL.UserID
                                        LEFT JOIN [Company] as C on U.CompanyID = C.CompanyID
                                        WHERE U.UserID=UL.UserID";

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
            User foundUser = FindByPrimaryEmail(username);

            if (foundUser == null)
                foundUser = FindByUserName(username);

            return foundUser;
        }

        private User FindByUserName(string username)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + " AND UL.Login = @Username";

                var user = db.Connection.Query<User>(query, new { Username = username }).FirstOrDefault();
                SetAccessLevel(user);
                return user;
            }
        }
        
        public User FindByPrimaryEmail(string username)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = "SELECT User_Login.UserId, users.[FirstName] FirstName, users.[LastName] LastName, users.[UserType], users.[ClientPortalTypeID] ClientPortalType, users.ClientPortalFTAlumniTypeID FloThruAlumniAccess, User_Login.[Login], User_Login.[Password] PasswordHash, C.[CompanyID] CompanyId, C.[CompanyName], C.[MaxVisibleRatePerHour] as ClientsMaxVisibleRate, C.[IsHavingFTAlumni] IsCompanyParticipating From     user_email INNER JOIN users ON users.userid = user_email.userid INNER JOIN [User_Login] ON [User_Login].userid = users.userid LEFT JOIN [Company] as C on users.CompanyID = C.CompanyID Where user_email.[PrimaryEmail] = @Username and User_Login.ForceUpdate = 0 and users.usertype in (select picklistid from dbo.udf_getpicklistids('userroles','Candidate,New Candidate',-1))";

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
            if (user != null && Settings.ShouldUseConfiguredParticipatingCompaniesList 
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
