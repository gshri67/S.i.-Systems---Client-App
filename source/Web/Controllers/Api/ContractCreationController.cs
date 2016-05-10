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
    [RoutePrefix("api/ContractCreation")]
    public class ContractCreationController: ApiController
    {
        
        private readonly ContractCreationService _service;
        
        public ContractCreationController(ContractCreationService service)
        {
            _service = service;
        }

        [Route("job/{jobId}/candidate/{candidateId}")]
        public HttpResponseMessage GetInitialContract(int jobId, int candidateId)
        {
            var contract = _service.GetContractDetailsByJobIdAndCandidateId(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, contract);
        }

        [Route("Review/job/{jobId}/candidate/{candidateId}")]
        public HttpResponseMessage GetDetailsForReviewContractCreationForm(int jobId, int candidateId)
        {
            var contract = _service.GetDetailsForReviewContractCreationForm(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, contract);
        }


        [Route("Submit/job/{jobId}/candidate/{candidateId}/contractDetails/{contractCreationDetails}")]
        public HttpResponseMessage SubmitContract(int jobId, int candidateId, ContractCreationDetails contractCreationDetails )
        {
            var result = _service.SubmitContract(jobId, candidateId, contractCreationDetails);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}
