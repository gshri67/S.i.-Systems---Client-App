using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class ConsultantDetailsService
    {
        private readonly IConsultantDetailsRepository _consultantDetailsRepository;
        private readonly ISessionContext _sessionContext;

        public ConsultantDetailsService(ISessionContext sessionContext, IConsultantDetailsRepository consultantDetailsRepository)
        {
            _consultantDetailsRepository = consultantDetailsRepository;
            _sessionContext = sessionContext;
        }

        public ConsultantDetails GetCurrentUserConsultantDetails()
        {
            return _consultantDetailsRepository.GetConsultantDetails(_sessionContext.CurrentUser.Id);
        }
    }
}
