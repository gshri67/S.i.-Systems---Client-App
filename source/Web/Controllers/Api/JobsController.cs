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
    [RoutePrefix("api/Jobs")]
    public class JobsController : ApiController
    {

        private readonly JobService _service;
        public  JobsController(JobService service)
        {
           _service = service;
        }

        [Route("Summaries")]
        public HttpResponseMessage GetJobSummaries()
        {
            var jobSummaries = _service.GetJobSummaries();
            return Request.CreateResponse(HttpStatusCode.OK, jobSummaries);
        }

        [Route("Client/{id}")]
        public HttpResponseMessage GetJobsByClientID(int id)
        {
            var jobs = _service.GetJobsByClientId(id);
            return Request.CreateResponse(HttpStatusCode.OK, jobs);
        }

        [Route("{id}")]
        public HttpResponseMessage GetJobWithJobId(int id)
        {
            var job = _service.GetJobWithJobId(id);
            job.Status = JobStatus.Shortlisted;
            
            return Request.CreateResponse(HttpStatusCode.OK, job);
        }
    }
}