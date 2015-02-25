using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<Specialization>> GetAllAsync();
    }

    public class SpecializationRepository: ISpecializationRepository
    {
        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                return await db.Connection.QueryAsync<Specialization>("SELECT SpecializationId Id, Name, Description FROM Specialization");
            }
        }

        public async Task<IEnumerable<Specialization>> FindAsync(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                return
                    await
                        db.Connection.QueryAsync<Specialization>(
                            "SELECT SpecializationId Id, Name, Description FROM Specialization WHERE SpecializationId = @Id",
                            new {Id = id});
            }
        }
    }
}
