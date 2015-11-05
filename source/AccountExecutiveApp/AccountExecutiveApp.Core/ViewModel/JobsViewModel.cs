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

        /*
        public string ClientName { get; set; }
        public string JobTitle { get; set; }
        public bool isProposed { get; set; }
        public bool hasCallout { get; set; }
        public DateTime issueDate { get; set; }
         */

        public JobsViewModel(IMatchGuideApi api)
		{
			this._api = api;
		}

        public async Task getJobs()
		{
			Jobs = await this._api.getJobs();
		}

        public void LoadJobs(Action jobsLoadedCallback)
        {
            var jobLoadingTask = getJobs();
            jobLoadingTask.ContinueWith(_ => JobInfoRetrieved());

            if (jobsLoadedCallback != null)
                jobLoadingTask.ContinueWith(_ => jobsLoadedCallback());
        }

        public void JobInfoRetrieved()
        {
            if (Jobs == null)
                Jobs = new List<Job>();
        }

        public string ClientName( int index )
        {
            if (Jobs != null)
                return Jobs.ElementAt(index).ClientName;
            return null;
        }
        public string JobTitle(int index)
        {
            if (Jobs != null)
                return Jobs.ElementAt(index).JobTitle;

            return null;
        }
        public bool IsProposed(int index)
        {
            if (Jobs != null)
                return Jobs.ElementAt(index).isProposed;

            return false;
        }
        public bool HasCallout(int index)
        {
            if (Jobs != null)
                return Jobs.ElementAt(index).hasCallout;

            return false;
        }
        public DateTime IssueDate(int index)
        {
            if (Jobs != null)
                return Jobs.ElementAt(index).issueDate;
            return DateTime.Now;
        }
	}
}
