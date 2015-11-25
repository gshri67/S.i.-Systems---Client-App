using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;
using SiSystems.ClientApp.Web.Filters;

namespace SiSystems.AccountExecutiveApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/Job")]
    public class JobController : ApiController
    {

        private readonly JobService _service;
        public  JobController(JobService service)
        {
           _service = service;
        }

        public HttpResponseMessage GetJobs()
        {
            var jobs = _service.GetJobs();
            return Request.CreateResponse(HttpStatusCode.OK, jobs);
        }

        [Route("{id}")]
        public HttpResponseMessage GetJobsByClientID(int id)
        {
            var jobs = _service.GetJobsByClientId(id);
            return Request.CreateResponse(HttpStatusCode.OK, jobs);
        }

        [Route("Details/{id}")]
        public HttpResponseMessage GetJobDetails(int id)
        {
            var job = _service.GetJobDetailsById(id);
            return Request.CreateResponse(HttpStatusCode.OK, job);
        }
    }
}