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

        [Route("Summary")]
        public HttpResponseMessage GetJobSummaries()
        {
            var jobSummaries = _service.GetJobSummaries();
            return Request.CreateResponse(HttpStatusCode.OK, jobSummaries);
        }

        [Route("{id}")]
        public HttpResponseMessage GetJobsByClientID(int id)
        {
            var jobs = _service.GetJobsByClientId(id);
            return Request.CreateResponse(HttpStatusCode.OK, jobs);
        }

        [Route("WithJobId/{id}")]
        public HttpResponseMessage GetJobWithJobId(int id)
        {
            var job = _service.GetJobWithJobId(id);
            job.Status = JobStatus.Shortlisted;
            
            return Request.CreateResponse(HttpStatusCode.OK, job);
        }

        [Route("Details/{id}")]
        public HttpResponseMessage GetJobDetails(int id)
        {
            var job = _service.GetJobDetailsById(id);
            return Request.CreateResponse(HttpStatusCode.OK, job);
        }


        [Route("Contractors/Shortlisted/{id}")]
        public HttpResponseMessage GetShortlistedContractorsByJobId(int id)
        {
            var contractors = _service.GetShortlistedContractorsByJobId(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractors);
        }
        [Route("Contractors/Proposed/{id}")]
        public HttpResponseMessage GetProposedContractorsByJobId(int id)
        {
            var contractors = _service.GetProposedContractorsByJobId(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractors);
        }
        [Route("Contractors/Callout/{id}")]
        public HttpResponseMessage GetCalloutContractorsByJobId(int id)
        {
            var contractors = _service.GetCalloutContractorsByJobId(id);
            return Request.CreateResponse(HttpStatusCode.OK, contractors);
        }
    }
}