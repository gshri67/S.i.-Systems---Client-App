using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class PayRateService
    {
        private readonly ISessionContext _sessionContext;
        private readonly IPayRateRepository _payRateRepository;

        public PayRateService(ISessionContext sessionContext, IPayRateRepository payRateRepository)
        {
            _sessionContext = sessionContext;
            _payRateRepository = payRateRepository;
        }

        public IEnumerable<PayRate> GetPayRates()
        {
            var payRates = _payRateRepository.GetPayRates().ToList();

            List<string> list = new List<string>();
            List<PayRate> ratesList = new List<PayRate>();
            

            foreach( PayRate payRate in payRates )
                if( payRate.RateDescription != null && !list.Contains(payRate.RateDescription) )
                    {
                       list.Add(payRate.RateDescription);//used to remove duplicate values (which shouldnt happen anyways)
                        ratesList.Add(payRate);
                    }  

            return ratesList;
        }
    }
}
