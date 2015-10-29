using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [RoutePrefix("api/ConsultantConstract")]
    public class ConsultantContractController : ApiController
    {

        private readonly ConsultantContractService _service;
        public ConsultantContractController(ConsultantContractService service)
        {
            _service = service;
        }

        public HttpResponseMessage getJobs()
        {
            var contracts = _service.GetContracts();
            return Request.CreateResponse(HttpStatusCode.OK, contracts);

        }
    }
}