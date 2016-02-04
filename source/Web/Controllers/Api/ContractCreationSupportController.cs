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
    [RoutePrefix("api/ContractSupport")]
    public class ContractCreationSupportController: ApiController
    {

        private readonly AccountExecutiveService _accountExecutiveService;

        public ContractCreationSupportController(AccountExecutiveService accountExecutiveService)
        {
            _accountExecutiveService = accountExecutiveService;
        }

        [Route("AccountExecutives")]
        public HttpResponseMessage GetAccountExecutives()
        {
            var accountExecutives = _accountExecutiveService.GetAccountExecutives();
            return Request.CreateResponse(HttpStatusCode.OK, accountExecutives);
        }
    }
}*/