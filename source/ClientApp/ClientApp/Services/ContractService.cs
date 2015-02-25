using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class ContractService : IContractService
    {
        private IConnectionService _connection;

        public ContractService(IConnectionService connection)
        {
            _connection = connection;
        }

        public Task<IList<Specialization>> GetAllSpecializations()
        {
            return Task.FromResult<IList<Specialization>>(new[]
                                                   {
                                                       new Specialization {Id = 1, Name = "Developer"},
                                                       new Specialization {Id = 4, Name = "Project Management"},
                                                       new Specialization {Id = 3, Name = "Senior Developer"},
                                                   });
        }
    }
}
