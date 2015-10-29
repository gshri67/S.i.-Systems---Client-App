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

        public DashboardSummary GetDashboardSummary()
        {
            var dashboardInfo = new DashboardSummary
            {
                FlowThruContracts = _contractsRepository.GetFlowThruSummary(),
                FullySourcedContracts = _contractsRepository.GetFullySourcedSummary(),
                Jobs = _jobsRepository.GetJobsSummary()
            };

            return dashboardInfo;
        }
    }
}
