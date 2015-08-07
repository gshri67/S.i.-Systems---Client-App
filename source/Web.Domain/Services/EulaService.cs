using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class EulaService
    {
        private readonly EulaRepository _repository;

        public EulaService(EulaRepository repository)
        {
            _repository = repository;
        }

        public Eula GetMostRecentEula()
        {
            return _repository.GetMostRecentEula();
        }
    }
}
