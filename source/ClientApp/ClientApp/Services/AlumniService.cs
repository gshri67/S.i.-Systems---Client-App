using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class AlumniService : IAlumniService
    {
        private readonly IConnectionService _connection;

        public AlumniService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query)
        {
            var json = await _connection.Get("consultants/alumni", query);
            var groups = JsonConvert.DeserializeObject<List<ConsultantGroup>>(json);
            return groups;
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            var json = await _connection.Get(string.Format("consultants/alumni/{0}", id));
            var consultant = JsonConvert.DeserializeObject<Consultant>(json);
            return consultant;
        }
    }
}
