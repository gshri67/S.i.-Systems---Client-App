using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class DashboardService
    {
        private readonly IJobsRepository _jobsRepository;
        private readonly IContractsRepository _contractsRepository;
        private readonly ISessionContext _session;

        public DashboardService(IJobsRepository jobsRepository, IContractsRepository contractsRepository, ISessionContext session)
        {
            _jobsRepository = jobsRepository;
            _contractsRepository = contractsRepository;
            _session = session;
        }

        public DashboardInfo GetDashboardSummary()
        {
            var info = new DashboardInfo
            {
                FS_curContracts = 55,
                FS_startingContracts = 17,
                FS_endingContracts = 12,
                FT_curContracts = 55,
                FT_startingContracts = 17,
                FT_endingContracts = 12,
                curJobs = 18,
                calloutJobs = 6,
                proposedJobs = 9
            };

            return info;
        }
    }
}
