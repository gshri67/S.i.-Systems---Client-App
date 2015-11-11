using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobsListViewModel
    {
        private IEnumerable<Job> _jobs;

        public IEnumerable<Job> Jobs
        {
            get { return _jobs ?? Enumerable.Empty<Job>(); }
            set { _jobs = value ?? Enumerable.Empty<Job>(); }
        } 

        public JobsListViewModel()
        {
            
        }

        public Task SetJobs(IEnumerable<Job> jobs)
        {
            var jobLoadingTask = LoadJobs(jobs);

            return jobLoadingTask;
        }

        private async Task LoadJobs(IEnumerable<Job> jobs)
        {
            _jobs = jobs.OrderByDescending(job => job.issueDate);
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
            var differenceInDays = DateTime.Now.Subtract(job.issueDate).Days;


            return TimePassedDescription(differenceInDays);
        }
        
        private static string TimePassedDescription(int daysSinceStart)
        {
            var subtitleText = "New";
            
            if (daysSinceStart == 1)
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

        public string JobStatusDescription(Job job)
        {
            if (job == null) return string.Empty;

            var status = "";
            if (job.isProposed)
                status = "Proposed";
            if (job.hasCallout)
                status = "Callout";
            return status;
        }
    }
}
