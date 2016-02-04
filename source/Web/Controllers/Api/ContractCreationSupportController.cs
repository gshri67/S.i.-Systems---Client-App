/*using System;
using System.Net;
using System.Net.Http;
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

        [Route("PossibleValues/Job/{jobId}/Candidate/{candidateId}")]
        public HttpResponseMessage GetContractCreationSupportOptions(int jobId, int candidateId)
        {
            var options = _contractCreationSupportService.GetContractOptionsForJobAndCandidate(jobId, candidateId);
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }

        [Route("AccountExecutives")]
        public HttpResponseMessage GetAccountExecutives()
        {
            var accountExecutives = _contractCreationSupportService.GetColleaguesForCurrentUser();
            return Request.CreateResponse(HttpStatusCode.OK, accountExecutives);
        }
    }
}*/
