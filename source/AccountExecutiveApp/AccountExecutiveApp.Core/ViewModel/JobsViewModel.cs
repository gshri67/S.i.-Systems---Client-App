using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobsViewModel
	{
		private readonly IMatchGuideApi _api;
        
        private IEnumerable<Job> _jobs;
        public IEnumerable<Job> Jobs
        {
            get { return _jobs ?? Enumerable.Empty<Job>(); }
            set { _jobs = value ?? Enumerable.Empty<Job>(); }
        }

        public JobsViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

        public async Task GetJobs()
		{
			Jobs = await _api.GetJobs();
		}

        public Task LoadJobs()
        {
            var jobLoadingTask = GetJobs();

            return jobLoadingTask;
        }
	}
}
