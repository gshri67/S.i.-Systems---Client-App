using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class ClientDetailsService
    {
        private readonly IClientDetailsRepository _clientDetailsRepository;

        public ClientDetailsService(IClientDetailsRepository clientDetailsRepository)
        {
            _clientDetailsRepository = clientDetailsRepository;
        }

        public ClientAccountDetails GetClientDetails(int id)
        {
            return _clientDetailsRepository.GetClientDetails(id);
        }
    }
}
