﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/PayRates")]
    public class PayRatesController: ApiController
    {
        private readonly PayRateService _service;
        public PayRatesController(PayRateService service)
        {
            _service = service;
        }

        [Route("{id}")]
        public HttpResponseMessage GetPayRates(int id)
        {
            var payRates = _service.GetPayRatesByContractId(id);
            return Request.CreateResponse(HttpStatusCode.OK, payRates);
        }

        [Route("TimesheetSupport/{id}")]
        public HttpResponseMessage GetTimesheetSupportByTimesheetId(int id)
        {
            var timesheetSupport = _service.GetTimesheetSupportByTimesheetId(id);
            return Request.CreateResponse(HttpStatusCode.OK, timesheetSupport);
        }

    }
}
