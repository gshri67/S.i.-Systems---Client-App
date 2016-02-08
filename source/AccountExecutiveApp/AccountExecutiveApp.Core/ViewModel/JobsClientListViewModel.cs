using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobsClientListViewModel
    {
        private IEnumerable<Job> _jobs;
        public IEnumerable<Job> Jobs
        {
            get { return _jobs ?? new List<Job>().AsEnumerable(); }
            set { _jobs = value ?? new List<Job>().AsEnumerable(); }
        }

        public JobsClientListViewModel()
        {
            Jobs = new List<Job>().AsEnumerable();
        }

        public void LoadJobs(IEnumerable<Job> jobs)
        {
            Jobs = jobs;
        }
    }
}
