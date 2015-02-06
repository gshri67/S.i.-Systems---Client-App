using System.Linq;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class EulaRepository
    {
        public Eula GetMostRecentEula()
        {
            using (var db = new DatabaseContext(DatabaseSelect.ClientApp))
            {
                const string query = @"SELECT TOP 1 [Version]
                                          ,[Text]
                                          ,[PublishedDate]
                                      FROM [Eula]
                                      ORDER BY Version DESC";

                return db.Connection.Query<Eula>(query).FirstOrDefault();
            }
        }
    }
}
