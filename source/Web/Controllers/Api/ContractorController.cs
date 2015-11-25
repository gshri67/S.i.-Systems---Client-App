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

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/Contractor")]
    public class ContractorController : ApiController
    {
        private readonly ContractorService _service;

        public ContractorController(ContractorService service)
        {
            _service = service;
        }

        [Route("{id}")]
        public HttpResponseMessage GetContractorById(int id)
        {
            var contractor = _service.GetContractorById(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractor);
        }
    }
}
