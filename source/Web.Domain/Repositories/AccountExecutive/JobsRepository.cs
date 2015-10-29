using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IJobsRepository
    {
        JobsSummarySet GetJobsSummary();
    }

    public class JobsRepository : IJobsRepository
    {
        public JobsSummarySet GetJobsSummary()
        {
            return new JobsSummarySet
            {
                All = 18,
                Proposed = 9,
                Callouts = 6
            };
        }
    }
}
