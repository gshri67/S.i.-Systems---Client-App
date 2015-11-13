using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobDetailViewModel
    {
        private readonly IMatchGuideApi _api;
        
        private JobDetails _jobDetails;

        private JobDetails JobDetails
        {
            get { return _jobDetails ?? new JobDetails(); }
            set { _jobDetails = value ?? new JobDetails(); }
        }

        public string ClientContactName
        {
            get
            {
                return string.Format(JobDetails.ClientContact.FullName);
            }
        }

        public string DirectReportName
        {
            get
            {
                return string.Format(JobDetails.DirectReport.FullName);
            }
        }

        public int NumberOfShortlistedConsultants
        {
            get { return JobDetails.Shortlisted.Count(); }
        }

        public int NumberOfProposedContractors
        {
            get { return JobDetails.Proposed.Count(); }
        }

        public int NumberOfContractorsWithCallouts
        {
            get { return JobDetails.Callouts.Count(); }
        }

        public string JobTitle
        {
            get { return JobDetails.Title; }
        }

        public IEnumerable<IM_Consultant> ShortlistedConsultants
        {
            get { return JobDetails.Shortlisted ?? Enumerable.Empty<IM_Consultant>(); }
        }

        public IEnumerable<IM_Consultant> ProposedConsultants
        {
            get { return JobDetails.Proposed ?? Enumerable.Empty<IM_Consultant>(); }
        }

        public IEnumerable<IM_Consultant> ConsultantsWithCallouts
        {
            get { return JobDetails.Callouts ?? Enumerable.Empty<IM_Consultant>(); }
        }

        public JobDetailViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        public Task LoadJob(Job job)
        {
            var task = GetJobDetails(job);

            return task;
        }

        private async Task GetJobDetails(Job job)
        {
            if (job == null) return;
            JobDetails = await _api.GetJobDetails(job.Id);
        }
    }
}
