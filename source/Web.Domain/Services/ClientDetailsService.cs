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
        private readonly ISessionContext _sessionContext;


        public ClientDetailsService(IClientDetailsRepository clientDetailsRepository, ISessionContext sessionContext)
        {
            _clientDetailsRepository = clientDetailsRepository;
            _sessionContext = sessionContext;
        }

        public ClientAccountDetails GetAccountDetails()
        {
            return _clientDetailsRepository.GetClientDetails(_sessionContext.CurrentUser.ClientId);
        }
    }
}
