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
            _contractsRepository.GetSummaryForDashboard();
            var info = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Current = 55,
                    Starting = 17,
                    Ending = 12
                },
                FullySourcedContracts = new ContractSummarySet
                {
                    Current = 55,
                    Starting = 17,
                    Ending = 12
                },
                Jobs = new JobsSummarySet
                {
                    All = 18,
                    Proposed = 9,
                    Callouts = 6
                }
            };

            return info;
        }
    }
}
