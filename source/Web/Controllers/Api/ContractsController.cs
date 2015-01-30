using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class ContractsController: ApiController
    {
        private readonly ContractService _service;
        public ContractsController(ContractService service)
        {
            _service = service;
        }

        public HttpResponseMessage Post(Contract contract)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
