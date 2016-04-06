using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.AccountExecutiveApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/ContractCreationSupport")]
    public class ContractCreationSupportController: ApiController
    {
        
        private readonly ContractCreationSupportService _contractCreationSupportService;
        
        public ContractCreationSupportController(ContractCreationSupportService contractCreationSupportService)
        {
            _contractCreationSupportService = contractCreationSupportService;
        }

        [Route("MainFormOptions")]
        public HttpResponseMessage GetMainFormOptions()
        {
            var options = _contractCreationSupportService.GetContractOptionsForMainForm();
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }
    }
}
