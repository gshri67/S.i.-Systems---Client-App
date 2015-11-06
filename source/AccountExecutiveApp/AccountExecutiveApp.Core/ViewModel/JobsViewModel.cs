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
        public IEnumerable<Job> Jobs;

        public JobsViewModel(IMatchGuideApi api)
		{
			this._api = api;
		}

        public async Task GetJobs()
		{
			Jobs = await this._api.getJobs();
		}

        public Task LoadJobs()
        {
            var jobLoadingTask = GetJobs();

            jobLoadingTask.ContinueWith(_ => JobInfoRetrieved());

            return jobLoadingTask;
        }

        public void JobInfoRetrieved()
        {
            if (Jobs == null)
                Jobs = new List<Job>();
        }
	}
}
