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
        
        private Job _job;

        public Job Job
        {
            get { return _job ?? new Job(); }
            private set { _job = value ?? new Job(); }
        }

        public string ClientContactName
        {
            get
            {
                return string.Format(Job.ClientContact.FullName);
            }
        }

		public string ClientName
		{
			get
			{
				return string.Format(Job.ClientName);
			}
		}

        public int NumberOfShortlistedConsultants
        {
            get { return Job.NumShortlisted; }
        }

        public int NumberOfProposedContractors
        {
            get { return Job.NumProposed; }
        }

        public int NumberOfContractorsWithCallouts
        {
            get { return Job.NumCallouts; }
        }

        public string JobTitle
        {
            get { return Job.Title; }
        }

		public int ClientContactId
		{
			get { return Job.ClientContact.Id; }
		}

        public JobDetailViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        public Task LoadJobWithJobID( int Id )
        {
            var task = GetJobWithJobId( Id );

            return task;
        }

        private async Task GetJobWithJobId( int Id )
        {
            Job = await _api.GetJobWithJobId(Id);
        }
    }
}
