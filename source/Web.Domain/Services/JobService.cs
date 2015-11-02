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

            for (int i = 0; i < 28; i++)
            {
                Job job = new Job();

                if (i < 10)
                    job.ClientName = "Nexen";
                else
                    job.ClientName = "Cenovus";

                job.isProposed = (i % 3) == 0;
                job.hasCallout = job.isProposed && ((i%2) == 0);
                job.JobTitle = "Developer" + i.ToString();

                job.issueDate = new DateTime(2015, 10, i + 1 + (i%5) - (i%3) );

                jobList.Add(job);
            }
            return jobList.AsEnumerable<Job>();
        }
    }
}
