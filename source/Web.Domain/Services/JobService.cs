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
        private readonly IUserContactRepository _userRepository;
        private readonly ISessionContext _sessionContext;

        public JobService(IJobsRepository jobsRepository, IUserContactRepository userRepository, ISessionContext sessionContext)
        {
            _jobsRepository = jobsRepository;
            _userRepository = userRepository;
            _sessionContext = sessionContext;
        }

        /// <summary>
        /// Validate that the current user is the Account Executive for the job being requested
        /// </summary>
        private void AssertCurrentUserHasPermissionsToViewJobDetails(Job job)
        {
            //todo: add business rules regarding who can see which job details
            if(false)
                throw new UnauthorizedAccessException();
        }

        public IEnumerable<Job> GetJobsByClientId(int id)
        {
            var jobs = _jobsRepository.GetJobsByClientIdAndAccountExecutiveId(id, _sessionContext.CurrentUser.Id);
            
            return jobs;
        }

        public Job GetJobWithJobId(int id)
        {
            var job = _jobsRepository.GetJobWithJobId(id);

            job.ClientContact = _userRepository.GetClientContactByAgreementId(job.Id);

            AssertCurrentUserHasPermissionsToViewJobDetails(job);

            return job;
        }

        public IEnumerable<JobSummary> GetJobSummaries()
        {
            return _jobsRepository.GetJobSummariesByAccountExecutiveId(_sessionContext.CurrentUser.Id);
        }
    }
}
