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

        [Route("Rates/Job/{jobId}/Candidate/{candidateId}")]
        public HttpResponseMessage GetContractCreationPayRatePageOptions(int jobId, int candidateId)
        {
            var options = new ContractCreationOptions_Rate();// _contractCreationSupportService.GetContractCreationPayRatePageOptions(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }

        [Route("Initial/Job/{jobId}/Candidate/{candidateId}")]
        public HttpResponseMessage GetContractCreationInitialPageOptions(int jobId, int candidateId)
        {
            var options = new ContractCreationOptions();// _contractCreationSupportService.GetContractCreationInitialPageOptions(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }

        [Route("Sending/Job/{jobId}/Candidate/{candidateId}")]
        public HttpResponseMessage GetContractCreationSendingPageOptions(int jobId, int candidateId)
        {
            var options = new ContractCreationOptions_Sending();// _contractCreationSupportService.GetContractCreationSendingPageOptions(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }
    }
}
