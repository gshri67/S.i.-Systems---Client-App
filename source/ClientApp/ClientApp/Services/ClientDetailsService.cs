using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class ClientDetailsService : IClientDetailsService
    {
        private readonly IConnectionService _connection;

        public ClientDetailsService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<ClientAccountDetails> GetClientDetails()
        {
            var json = await _connection.Get("ClientDetails");
            var accountDetails = JsonConvert.DeserializeObject<ClientAccountDetails>(json);
            return accountDetails;
        }
    }
}
