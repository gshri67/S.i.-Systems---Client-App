using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain
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
