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
    public class ContractorsController : ApiController
    {
        private readonly ContractorsService _service;

        public ContractorsController(ContractorsService service)
        {
            _service = service;
        }

        [Route("{id}")]
        public HttpResponseMessage GetContractorById(int id)
        {
            var contractor = _service.GetContractorById(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractor);
        }

        [Route("Job/{id}/Status/{status}")]
        public HttpResponseMessage GetContractorsByJobIdAndStatus( int id, JobStatus status )
        {
            var contractors = _service.GetContractorsByJobIdAndStatus(id, status);
            return Request.CreateResponse(HttpStatusCode.OK, contractors);
        }
    }
}
