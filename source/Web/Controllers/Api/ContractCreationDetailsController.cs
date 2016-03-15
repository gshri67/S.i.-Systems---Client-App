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
    [RoutePrefix("api/Contractors")]
    public class ContractCreationDetailsController : ApiController
    {
        private readonly ContractorsService _service;

        public ContractCreationDetailsController(ContractorsService service)
        {
            _service = service;
        }

        [Route("Rates/Job/{jobId}/Candidate/{candidateId}")]
        public HttpResponseMessage GetContractCreationPayRatePageDetails(int jobId, int candidateId)
        {
            var details = new ContractCreationDetails_Rate();// _service.GetContractCreationPayRatePageDetails(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, details);
        }
    }
}
