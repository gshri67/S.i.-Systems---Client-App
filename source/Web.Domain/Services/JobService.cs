using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
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
        private readonly IJobsRepository _jobsRepository;

        public JobService(IJobsRepository jobsRepository)
        {
            _jobsRepository = jobsRepository;
        }

        public IEnumerable<Job> GetJobs()
        {
            return _jobsRepository.GetJobs();
        }

        public JobDetails GetJobDetailsById(int id)
        {
            var jobDetails = _jobsRepository.GetJobDetailsById(id);

            return jobDetails;
        }
    }
}
