using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class JobsClientListTableViewModel
    {
        private readonly IEnumerable<IGrouping<string, Job>> _groupedJobs;

        public JobsClientListTableViewModel(IEnumerable<Job> jobs)
        {

            _groupedJobs = jobs.GroupBy(job => job.ClientName) ?? new List<IGrouping<string, Job>>().AsEnumerable();
        }

        public int NumberOfGroups()
        {
            return _groupedJobs.Count();
        }

        public string ClientNameByRowNumber(int groupNumber)
        {
            return _groupedJobs.ElementAtOrDefault(groupNumber).Key;
        }

        public int NumberOfJobsInSection(int groupNumber)
        {
            return _groupedJobs.ElementAtOrDefault(groupNumber).Count();
        }

        public int NumberOfProposedJobsInSection(int groupNumber)
        {
            return _groupedJobs.ElementAtOrDefault(groupNumber).Count(job => job.isProposed);
        }

        public int NumberOfJobsWithCalloutsInSection(int groupNumber)
        {
            return _groupedJobs.ElementAtOrDefault(groupNumber).Count(job => job.hasCallout);
        }

        public string JobStateCountsByRowNumber(int groupNumber)
        {
            var numJobs = NumberOfJobsInSection(groupNumber);
            var jobsProposed = NumberOfProposedJobsInSection(groupNumber);
            var jobCallouts = NumberOfJobsWithCalloutsInSection(groupNumber);

            return string.Format("{0}/{1}/{2}", numJobs, jobsProposed, jobCallouts);
        }

        public IEnumerable<Job> JobsByRowNumber(int groupNumber)
        {
            return _groupedJobs.ElementAtOrDefault(groupNumber);
        }
    }
}
