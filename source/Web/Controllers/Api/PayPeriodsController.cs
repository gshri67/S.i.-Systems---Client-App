using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    //[ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/PayPeriods")]
    public class PayPeriodsController: ApiController
    {
        private readonly PayPeriodService _service;
        public PayPeriodsController(PayPeriodService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetPayPeriods()
        {
            var payPeriods = _service.GetRecentPayPeriods();
            return Request.CreateResponse(HttpStatusCode.OK, payPeriods);
        }

        [Route("Summaries")]
        public HttpResponseMessage GetPayPeriodSummaries()
        {
            var payPeriods = _service.GetRecentPayPeriodSummaries();
            return Request.CreateResponse(HttpStatusCode.OK, payPeriods);
        }
    }
}
