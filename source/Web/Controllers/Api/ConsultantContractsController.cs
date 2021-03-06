﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.AccountExecutiveApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/ConsultantContracts")]
    public class ConsultantContractsController : ApiController
    {
        private readonly ConsultantContractService _service;
        public ConsultantContractsController(ConsultantContractService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetContracts()
        {
            var contracts = _service.GetContractSummariesForAccountExecutive();
            return Request.CreateResponse(HttpStatusCode.OK, contracts);
        }

        [Route("Details/{id}")]
        public HttpResponseMessage GetContractDetailsById(int id)
        {
            var contractDetails = _service.GetContractDetailsById(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractDetails);
        }
    }
}