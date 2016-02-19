﻿using System;
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

        [Route("pdf/{docNumber}")]
        public HttpResponseMessage GetPDF( string docNumber )
        {
            var pdf = _myAccountService.RequestERemittancePDF("132241", "328584", "16017.75000", "10/30/2015", "pam sipar");
            return Request.CreateResponse(HttpStatusCode.OK, pdf);

            //var remittances = _service.GetRemittances();
            //return Request.CreateResponse(HttpStatusCode.OK, remittances);
        }
        
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post()
        {
            var result = await _myAccountService.RequestERemittancePDF("132241", "328584", "pam", "2015-10-30", "sipar");
            return result;
        }
    }
}
