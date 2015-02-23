using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    public class ContractsController: ApiController
    {
        private readonly ContractService _service;
        public ContractsController(ContractService service)
        {
            _service = service;
        }

        public HttpResponseMessage Post(ContractProposal contract)
        {
            _service.AddContractProposal(contract);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
