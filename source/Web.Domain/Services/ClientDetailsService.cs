using SiSystems.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class ClientDetailsService
    {
        private readonly IClientDetailsRepository _clientDetailsRepository;
        private readonly ISessionContext _sessionContext;

        public ClientDetailsService(ISessionContext sessionContext, IClientDetailsRepository clientDetailsRepository)
        {
            _clientDetailsRepository = clientDetailsRepository;
            _sessionContext = sessionContext;
        }

        public ClientAccountDetails GetClientDetails()
        {
            return _clientDetailsRepository.GetClientDetails(_sessionContext.CurrentUser.CompanyId);
        }
    }
}
