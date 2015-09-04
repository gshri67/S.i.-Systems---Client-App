using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class PayRateService
    {
        private readonly ISessionContext _sessionContext;

        public PayRateService(ISessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }

        public IEnumerable<string> GetPayRates()
        {
            List<String> list = new List<string>();
            list.Add("Regular");
            list.Add("Overtime");
            list.Add("Special Rate");

            IEnumerable<string> enumerableList = list.AsEnumerable();
            return enumerableList;
        }
    }
}
