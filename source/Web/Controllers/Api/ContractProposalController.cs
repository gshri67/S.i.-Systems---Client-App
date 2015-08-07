using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    public class ContractProposalController: ApiController
    {
        private readonly ContractProposalService _service;
        public ContractProposalController(ContractProposalService service)
        {
            _service = service;
        }

        public HttpResponseMessage Post(ContractProposal contract)
        {
            _service.SendProposal(contract);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
