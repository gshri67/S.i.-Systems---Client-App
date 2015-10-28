using System;
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

namespace SiSystems.ClientApp.Web.Controllers.Api
{
     [RoutePrefix("api/Job")]
    public class JobController : ApiController
    {

        private readonly JobService _service;
        public  JobController(JobService service)
        {
           _service = service;
        }

        public HttpResponseMessage getJobs()
        {
            var jobs = _service.GetJobs();
            return Request.CreateResponse(HttpStatusCode.OK, jobs);

        }
    }
}