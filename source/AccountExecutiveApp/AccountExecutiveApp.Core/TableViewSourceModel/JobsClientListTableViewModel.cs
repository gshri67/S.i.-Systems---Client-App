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
        private IEnumerable<JobSummary> _jobSummaries; 
        public IEnumerable<JobSummary> JobSummaries
        {
            get { return _jobSummaries ?? Enumerable.Empty<JobSummary>(); }
            set { _jobSummaries = value ?? Enumerable.Empty<JobSummary>(); }
        }

        public JobsClientListTableViewModel(IEnumerable<JobSummary> jobs)
        {
            JobSummaries = jobs;
        }

        public bool IndexIsInBounds(int index)
        {
            return index < JobSummaries.Count() && index >= 0;
        }

        public int NumberOfGroups()
        {
            return JobSummaries.Count();
        }

        public string ClientNameByRowNumber(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? JobSummaries.ElementAtOrDefault(groupNumber).ClientName
                : string.Empty;
        }

        public int ClientIDByRowNumber(int groupNumber)
        {
            return IndexIsInBounds(groupNumber)
                ? JobSummaries.ElementAtOrDefault(groupNumber).ClientId
                : 0;
        }

        public int NumberOfJobsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? JobSummaries.ElementAt(groupNumber).NumJobs
                : 0;
        }

        public int NumberOfProposedJobsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? JobSummaries.ElementAt(groupNumber).NumProposed
                : 0;
        }

        public int NumberOfJobsWithCalloutsInSection(int groupNumber)
        {
            return IndexIsInBounds(groupNumber) 
                ? JobSummaries.ElementAt(groupNumber).NumCallouts
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
    }
}
