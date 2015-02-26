using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class ContractService : IContractService
    {
        private readonly IConnectionService _connection;

        public ContractService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            var json = await _connection.Get("specializations");
            var specializations = JsonConvert.DeserializeObject<IEnumerable<Specialization>>(json);
            return specializations;
        }
    }
}
