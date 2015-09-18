using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class RemittanceService
    {
        private readonly ISessionContext _sessionContext;
        private readonly IRemittanceRepository _remittanceRepository;

        public RemittanceService(ISessionContext sessionContext, IRemittanceRepository remittanceRepository)
        {
            _sessionContext = sessionContext;
            _remittanceRepository = remittanceRepository;
        }

        public IEnumerable<Remittance> GetRemittances()
        {
            return _remittanceRepository.GetRemittancesForUser(_sessionContext.CurrentUser.Id);
        }

        public HttpResponseMessage GetPDF(string docNumber)
        {
            return null; 
        }
    }
}
