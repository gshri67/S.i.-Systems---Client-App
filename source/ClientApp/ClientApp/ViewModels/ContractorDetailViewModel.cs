﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class ContractorDetailViewModel : ViewModelBase
    {
        private readonly IAlumniService _alumniService;
        private Consultant _consultant;

        public ContractorDetailViewModel(IAlumniService alumniService)
        {
            _alumniService = alumniService;
        }

        public Task<Consultant> GetConsultant(int id)
        {
            return _alumniService.GetConsultant(id);
        }

    }
}
