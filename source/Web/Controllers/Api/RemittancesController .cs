using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

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

        [AllowAnonymous]
        public async Task<IHttpActionResult> Post()
        {
            var result = await _myAccountService.RequestERemittancePDF("191844", "330567", "pam", "2015-11-20", "sipar");
            return Ok(result);
        }

        /*
        public async Task<IHttpActionResult> Post([FromBody]string emailAddress)
        {
            var result = await this._service.ForgotPassword(emailAddress);
            return Ok(result);
        }
         */
    }
}
