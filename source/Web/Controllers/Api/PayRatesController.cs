using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/PayRates")]
    public class PayRatesController: ApiController
    {
        private readonly PayRateService _service;
        public PayRatesController(PayRateService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetPayRates()
        {
            var payRates = _service.GetPayRates();
            return Request.CreateResponse(HttpStatusCode.OK, payRates);
        }
    }
}
