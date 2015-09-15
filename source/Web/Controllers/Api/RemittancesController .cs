﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/Remittances")]
    public class RemittancesController: ApiController
    {
        private readonly RemittanceService _service;
        public RemittancesController(RemittanceService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetRemittances()
        {
            var remittances = _service.GetRemittances();
            return Request.CreateResponse(HttpStatusCode.OK, remittances);
        }

        [Route("pdf/{docNumber}")]
        public HttpResponseMessage GetPDF( string docNumber )
        {
            var pdf = _service.GetPDF();
            return Request.CreateResponse(HttpStatusCode.OK, pdf);
        }
    }
}
