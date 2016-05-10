using SiSystems.ClientApp.Web.Domain.Repositories;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class AnalyticsService
    {
        private readonly AnalyticsRepository _repository;
        public AnalyticsService(AnalyticsRepository repository)
        {
            _repository = repository;
        }

        public int TrackUserLogin( int userId, bool loginSuccessful)
        {
            return _repository.TrackUserLogin( userId, loginSuccessful);
        }
    }
}
