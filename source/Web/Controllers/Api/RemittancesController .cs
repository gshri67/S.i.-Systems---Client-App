using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/Remittances")]
    public class RemittancesController: ApiController
    {
        private readonly RemittanceService _service;
        private readonly MyAccountService _myAccountService;

        public RemittancesController(RemittanceService service, MyAccountService myAccountService)
        {
            _service = service;
            _myAccountService = myAccountService;
        }

        public HttpResponseMessage GetRemittances()
        {
            var remittances = _service.GetRemittances();
            return Request.CreateResponse(HttpStatusCode.OK, remittances);
        }

        [Route("pdf/remittanceVar")]
        public async Task<HttpResponseMessage> Post(Remittance rm)
        {
            var result = await _myAccountService.RequestERemittancePDF(rm);

            return result;
        }
    }
}
