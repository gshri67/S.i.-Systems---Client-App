using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class ContractorViewModel : ViewModelBase
    {
        private readonly IAlumniService _alumnService;

        public ContractorViewModel(IAlumniService alumnService)
        {
            _alumnService = alumnService;
        }

        public Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query)
        {
            return _alumnService.GetConsultantGroups(query);
        }
    }
}
