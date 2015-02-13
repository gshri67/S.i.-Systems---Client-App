using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class AlumniViewModel : ViewModelBase
    {
        private readonly IAlumniService _alumniService;
        private readonly ILogoutService _logoutService;

        public AlumniViewModel(IAlumniService alumniService, ILogoutService logoutService)
        {
            _alumniService = alumniService;
            _logoutService = logoutService;
        }

        public Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query)
        {
            return _alumniService.GetConsultantGroups(query);
        }

        public void Logout()
        {
            _logoutService.Logout();
        }
    }
}
