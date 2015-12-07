using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobsListViewModel
    {
        private IEnumerable<Job> _jobs;
        private readonly IMatchGuideApi _api;
        
        public IEnumerable<Job> Jobs
        {
            get { return _jobs ?? Enumerable.Empty<Job>(); }
            set { _jobs = value ?? Enumerable.Empty<Job>(); }
        } 

        public JobsListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

        public Task SetClientID( int ClientID )
        {
            var jobLoadingTask = LoadJobsWithClientID( ClientID );

            return jobLoadingTask;
        }

        private async Task GetJobs( int ClientID )
        {
            Jobs = await _api.GetJobsWithClientID(ClientID);
            _jobs = Jobs.OrderByDescending(job => job.IssueDate);
        }

        public Task LoadJobsWithClientID( int ClientID )
        {
            var jobLoadingTask = GetJobs( ClientID );

            return jobLoadingTask;
        }

        public string JobTitleByIndex(int index)
        {
            return IsInBounds(index)
                ? Jobs.ElementAt(index).JobTitle
                : string.Empty;
        }

        private bool IsInBounds(int index)
        {
            return index >= 0 && index < Jobs.Count();
        }

        public string TimeDescriptionByIndex(int rowNumber)
        {
            if (!IsInBounds(rowNumber)) return string.Empty;

            var job = Jobs.ElementAt(rowNumber);
			var differenceInDays = DateTime.UtcNow.Subtract(job.IssueDate).Days;


            return TimePassedDescription(differenceInDays);
        }
        
        private static string TimePassedDescription(int daysSinceStart)
        {
            var subtitleText = "";
            
			if (daysSinceStart < 1)
				subtitleText = "Today";
			else if (daysSinceStart == 1)
                subtitleText = "1 day ago";
            else if (daysSinceStart < 7)
                subtitleText = string.Format("{0} days ago", daysSinceStart);
            else if (daysSinceStart < 14)
                subtitleText = "1 week ago";
            else if (daysSinceStart < 30)
                subtitleText = string.Format("{0} weeks ago", daysSinceStart / 7 );
            else if (daysSinceStart < 60)
                subtitleText = "1 month ago";
            else if (daysSinceStart >= 60)
                subtitleText = string.Format("{0} months ago", daysSinceStart / 30);

            return subtitleText;
        }

        public string JobStatusByIndex(int rowNumber)
        {
            return IsInBounds(rowNumber) 
                ? JobStatusDescription(Jobs.ElementAtOrDefault(rowNumber))
                : string.Empty;
        }
		public string FormattedJobStatusRatioByIndex( int rowNumber )
		{
			return IsInBounds(rowNumber) 
				? string.Format("{0}/{1}/{2}", _jobs.ElementAt(rowNumber).NumShortlisted, _jobs.ElementAt(rowNumber).NumProposed, _jobs.ElementAt(rowNumber).NumCallouts )
					: string.Empty;
		}

        public string JobStatusDescription(Job job)
        {
            if (job == null) return string.Empty;

            var status = "";
            if (job.IsProposed)
                status = "Proposed";
            if (job.HasCallout)
                status = "Callout";
            return status;
        }

        public Job JobByIndex(int index)
        {
            return IsInBounds(index)
                ? Jobs.ElementAtOrDefault(index)
                : new Job();
        }
    }
}
