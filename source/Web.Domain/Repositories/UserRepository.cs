using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class UserRepository
    {
        public User Find(int id)
        {
            using (var db = new DatabaseContext())
            {
                const string query = "SELECT [Users].[UserId] Id, [Users].[CompanyID] ClientId, "
                                 + "[Users].[FirstName] FirstName, [Users].[LastName] LastName, "
                                 + "[User_Login].[Login] Login, [User_Login].[Password] PasswordHash "
                                 + "FROM [User_Login], [Users] "
                                 + "WHERE [Users].[UserID]=[User_Login].[UserID]"
                                 + "AND [Users].[UserId] = @Id";

                var user = db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();

                return user;
            }
        }

        public User FindByName(string username)
        {
            using (var db = new DatabaseContext())
            {
                const string query = "SELECT [Users].[UserId] Id, [Users].[CompanyID] ClientId, "
                                 + "[Users].[FirstName] FirstName, [Users].[LastName] LastName, "
                                 + "[User_Login].[Login] Login, [User_Login].[Password] PasswordHash "
                                 + "FROM [User_Login], [Users] "
                                 + "WHERE [Users].[UserID]=[User_Login].[UserID]"
                                 + "AND [User_Login].[Login] = @Username";

                var user = db.Connection.Query<User>(query, new { Username = username }).FirstOrDefault();
                return user;
            }
        }
    }
}
