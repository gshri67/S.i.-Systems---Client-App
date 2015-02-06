using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class UserRepository
    {
        const string UserQueryBase = "SELECT [Users].[UserId] Id, [Users].[CompanyID] ClientId, "
                               + "[Users].[FirstName] FirstName, [Users].[LastName] LastName, "
                               + "[User_Login].[Login] Login, [User_Login].[Password] PasswordHash "
                               + "FROM [User_Login], [Users] "
                               + "WHERE [Users].[UserID]=[User_Login].[UserID] ";

        public User Find(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + "AND [Users].[UserId] = @Id";

                return db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();
            }
        }

        public User FindByName(string username)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + "AND [User_Login].[Login] = @Username";

                return db.Connection.Query<User>(query, new {Username = username}).FirstOrDefault();
            }
        }
    }
}
