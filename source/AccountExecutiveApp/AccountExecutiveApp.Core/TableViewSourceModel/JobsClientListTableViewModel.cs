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
        private IEnumerable<IGrouping<string, Job>> _groupedJobs;
        private IEnumerable<IGrouping<string, Job>> GroupedJobs
        {
            get { return _groupedJobs ?? Enumerable.Empty<IGrouping<string, Job>>(); }
            set { _groupedJobs = value ?? Enumerable.Empty<IGrouping<string, Job>>(); }
        }

        public JobsClientListTableViewModel(IEnumerable<Job> jobs)
        {
            GroupedJobs = jobs == null 
                ? Enumerable.Empty<IGrouping<string, Job>>() 
                : jobs.GroupBy(job => job.ClientName);
        }

        public bool IndexIsInBounds(int index)
        {
            return index < GroupedJobs.Count() && index >= 0;
        }

        public int NumberOfGroups()
        {
            return GroupedJobs.Count();
        }

        public string ClientNameByRowNumber(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? GroupedJobs.ElementAtOrDefault(groupNumber).Key 
                : string.Empty;
        }

        public int NumberOfJobsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber)
                ? GroupedJobs.ElementAtOrDefault(groupNumber).Count()
                : 0;
        }

        public int NumberOfProposedJobsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber)
                ? GroupedJobs.ElementAtOrDefault(groupNumber).Count(job => job.isProposed)
                : 0;
        }

        public int NumberOfJobsWithCalloutsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? GroupedJobs.ElementAtOrDefault(groupNumber).Count(job => job.hasCallout)
                : 0;
        }

        public string JobStateCountsByRowNumber(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? string.Format("{0}/{1}/{2}", 
                    NumberOfJobsInSection(groupNumber), 
                    NumberOfProposedJobsInSection(groupNumber), 
                    NumberOfJobsWithCalloutsInSection(groupNumber))
                : string.Empty;
        }

        public IEnumerable<Job> JobsByRowNumber(int groupNumber)
        {
            return IndexIsInBounds(groupNumber)
                ? GroupedJobs.ElementAtOrDefault(groupNumber)
                : Enumerable.Empty<Job>();
        }
    }
}
