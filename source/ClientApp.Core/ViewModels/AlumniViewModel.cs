using System.Collections.Generic;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Core.ViewModels
{
    public class AlumniViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;

        public AlumniViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query)
        {
            return _api.GetConsultantGroups(query);
        }

        public async Task Logout()
        {
            await _api.Logout();
        }
    }
}
