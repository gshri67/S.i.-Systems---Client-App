
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/Analytics")]
    public class AnalyticsController : ApiController
    {
        private readonly AnalyticsService _service;

        public AnalyticsController(AnalyticsService service)
        {
            _service = service;
        }

        [Route("OpenedApp")]
        [HttpPost]
        public HttpResponseMessage OpenedApp()
        {
            var result = _service.ApplicationOpened();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("TrackContractCreated/{agreementId}")]
        public HttpResponseMessage TrackContractCreatedWithinApp( int agreementId )
        {
            var result = _service.TrackContractCreatedWithinApp(agreementId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
