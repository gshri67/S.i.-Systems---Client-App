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

        public async Task Submit(ContractProposal proposal)
        {
            await _connection.Post("ContractProposal", proposal);
        }
    }
}
