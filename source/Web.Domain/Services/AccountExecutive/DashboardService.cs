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
        private readonly IConsultantContractRepository _contractsRepository;
        private readonly ISessionContext _session;

        public DashboardService(IJobsRepository jobsRepository, IConsultantContractRepository contractsRepository, ISessionContext session)
        {
            _jobsRepository = jobsRepository;
            _contractsRepository = contractsRepository;
            _session = session;
        }

        public DashboardSummary GetDashboardSummary()
        {
            var dashboardInfo = new DashboardSummary
            {
                FlowThruContracts = _contractsRepository.GetFlowThruSummaryByAccountExecutiveId(_session.CurrentUser.Id),
                FullySourcedContracts = _contractsRepository.GetFullySourcedSummaryByAccountExecutiveId(_session.CurrentUser.Id),
                Jobs = _jobsRepository.GetJobsSummaryByAccountExecutiveId(_session.CurrentUser.Id)
            };

            return dashboardInfo;
        }
    }
}
