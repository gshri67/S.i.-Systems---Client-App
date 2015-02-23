using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class UserRepository
    {
        const string UserQueryBase = "SELECT U.UserId Id, U.CompanyID ClientId, C.CompanyName, "
                               + "U.FirstName FirstName, U.LastName LastName, "
                               + "UL.Login Login, UL.Password PasswordHash "
                               + "FROM [User_Login] as UL, [Users] as U, [Company] as C "
                               + "WHERE U.UserID=UL.UserID "
                               + "AND U.CompanyID=C.CompanyID ";

        public User Find(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + "AND U.UserId = @Id";

                return db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();
            }
        }

        public User FindByName(string username)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = UserQueryBase
                                    + "AND UL.Login = @Username";

                return db.Connection.Query<User>(query, new {Username = username}).FirstOrDefault();
            }
        }
    }
}
