using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class ConsultantDetailViewModel : ViewModelBase
    {
        private readonly IAlumniService _alumniService;

        private Consultant _consultant;

        public ConsultantDetailViewModel(IAlumniService alumniService)
        {
            _alumniService = alumniService;
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            if (_consultant != null && _consultant.Id == id)
            {
                return _consultant;
            }

            _consultant = await _alumniService.GetConsultant(id);
            return _consultant;
        }

        public Consultant GetConsultant()
        {
            return _consultant ?? new Consultant();
        }

    }
}
