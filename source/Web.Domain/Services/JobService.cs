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
        private readonly ISessionContext _sessionContext;

        public JobService(IJobsRepository jobsRepository, ISessionContext sessionContext)
        {
            _jobsRepository = jobsRepository;
            _sessionContext = sessionContext;
        }

        public IEnumerable<Job> GetJobs()
        {
            return _jobsRepository.GetJobsByAccountExecutiveId(_sessionContext.CurrentUser.Id);
        }

        /// <summary>
        /// Validate that the current user is the Account Executive for the job being requested
        /// </summary>
        private void AssertCurrentUserHasPermissionsToViewJobDetails(JobDetails jobDetails)
        {
            //todo: add business rules regarding who can see which job details
            if(false)
                throw new UnauthorizedAccessException();
        }

        public JobDetails GetJobDetailsById(int id)
        {
            var jobDetails = _jobsRepository.GetJobDetailsByJobId(id);

            AssertCurrentUserHasPermissionsToViewJobDetails(jobDetails);

            return jobDetails;
        }
    }
}
