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

        public Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query)
        {
            return _api.GetAlumniConsultantGroups(query);
        }
        
        public Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query)
        {
            return _api.GetActiveConsultantGroups(query);
        }

        public async Task Logout()
        {
            await _api.Logout();
        }
        
        public Task<ClientAccountDetails> GetClientDetailsAsync()
        {
            return _api.GetClientDetails();
        }
    }
}
