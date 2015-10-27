using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class JobService
    {

        public JobService()
        {
        }

        public IEnumerable<Job> GetJobs() 
        {
            List<Job> jobList = new List<Job>();

            Job job = new Job();
            job.ClientName = "Nexen";
            jobList.Add(job);

            return jobList.AsEnumerable<Job>();
        }
    }
}
