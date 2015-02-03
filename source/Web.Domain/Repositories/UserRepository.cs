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
                const string query = "SELECT * FROM Users WHERE Id = @Id";

                var user = db.Connection.Query<User>(query, new { Id = id }).FirstOrDefault();

                return user;
            }
        }

        public User FindByName(string username)
        {
            using (var db = new DatabaseContext())
            {
                const string query = "SELECT * FROM Users WHERE EmailAddress = @EmailAddress";

                var user = db.Connection.Query<User>(query, new { EmailAddress = username }).FirstOrDefault();
                return user;
            }
        }
    }
}
