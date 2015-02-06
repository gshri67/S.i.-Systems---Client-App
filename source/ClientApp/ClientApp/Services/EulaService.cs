using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class EulaService : IEulaService
    {
        private readonly IConnectionService _connection;

        public EulaService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<Eula> GetMostRecentEula()
        {
            var json = await _connection.Get("Eula");
            var eula = JsonConvert.DeserializeObject<Eula>(json);
            return eula;
        }
    }
}
