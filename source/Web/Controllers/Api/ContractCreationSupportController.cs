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

        [Route("MainFormOptions/{jobId}")]
        public HttpResponseMessage GetMainFormOptions( int jobId )
        {
            var options = _contractCreationSupportService.GetContractOptionsForMainForm( jobId );
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }

        [Route("SendingFormOptions/{jobId}")]
        public HttpResponseMessage GetSendingFormOptions(int jobId)
        {
            var options = _contractCreationSupportService.GetContractOptionsForSendingForm(jobId);
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }

        [Route("RatesFormOptions")]
        public HttpResponseMessage GetRateOptions()
        {
            var options = _contractCreationSupportService.GetContractOptionsForRates();
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }
    }
}
