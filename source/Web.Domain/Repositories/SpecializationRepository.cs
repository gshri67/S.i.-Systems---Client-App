using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<Specialization>> GetAllAsync();

        Task<Specialization> FindAsync(int id);
    }

    public class SpecializationRepository: ISpecializationRepository
    {
        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                return await db.Connection.QueryAsync<Specialization>("SELECT SpecializationId Id, Name, Description FROM Specialization WHERE VerticalId = 4");
            }
        }

        public async Task<Specialization> FindAsync(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var specializations =
                    await
                        db.Connection.QueryAsync<Specialization>(
                            "SELECT TOP 1 SpecializationId Id, Name, Description FROM Specialization WHERE SpecializationId = @Id",
                            new {Id = id});
                return specializations.FirstOrDefault();
            }
        }
    }
}
