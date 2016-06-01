﻿using SiSystems.ClientApp.Web.Domain.Repositories;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class AnalyticsService
    {
        private readonly AnalyticsRepository _repository;
        private readonly ISessionContext _context;

        public AnalyticsService(AnalyticsRepository repository, ISessionContext context)
        {
            _repository = repository;
            _context = context;
        }

        public int TrackUserLogin( bool loginSuccessful)
        {
            string userLogin = _context.CurrentUser.Login;
            return _repository.TrackUserLogin( userLogin, loginSuccessful);
        }

        public int TrackContractCreatedWithinApp(int agreementId)
        {
            return _repository.TrackContractCreatedWithinApp(agreementId);
        }
    }
}
