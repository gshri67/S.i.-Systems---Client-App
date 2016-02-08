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
    [RoutePrefix("api/ContractorRate")]
    public class ContractorRateController : ApiController
    {
        private readonly ContractorRateService _service;

        public ContractorRateController(ContractorRateService service)
        {
            _service = service;
        }

        [Route("Job/{id}/Status/{status}")]
        public HttpResponseMessage GetContractorRateSummaryWithJobIdAndStatus(int id, JobStatus status )
        {
            var rateSummary = _service.GetContractorRateSummaryWithJobIdAndStatus(id, status);
            return Request.CreateResponse(HttpStatusCode.OK, rateSummary);
        }
    }
}
