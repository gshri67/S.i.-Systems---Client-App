using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class UserRepository
    {
        private IDbConnection CreateDbConnection()
        {
            var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationDb"].ConnectionString);
            return connection;

        }

        public User Find(int id)
        {
            using (var connection = CreateDbConnection())
            {
                connection.Open();

                const string query = "SELECT * FROM Users WHERE Id = @Id";

                var user = connection.Query<User>(query, new { Id = id }).FirstOrDefault();
                return user;
            }
        }

        public User FindByName(string username)
        {
            using (var connection = CreateDbConnection())
            {
                connection.Open();

                const string query = "SELECT * FROM Users WHERE EmailAddress = @EmailAddress";

                var user = connection.Query<User>(query, new { EmailAddress = username }).FirstOrDefault();
                return user;
            }
        }
    }
}
