using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class DashboardService
    {
        private readonly IJobsRepository _jobsRepository;
        private readonly IConsultantContractRepository _contractsRepository;
        private readonly ISessionContext _session;
        private readonly ITimesheetRepository _timesheetsRepository;

        public DashboardService(IJobsRepository jobsRepository, IConsultantContractRepository contractsRepository, ITimesheetRepository timesheetsRepository, ISessionContext session)
        {
            _jobsRepository = jobsRepository;
            _contractsRepository = contractsRepository;
            _session = session;
            _timesheetsRepository = timesheetsRepository;
        }

        public DashboardSummary GetDashboardSummary()
        {
            var dashboardInfo = new DashboardSummary
            {
				UserName = _session.CurrentUser.FullName,
                FlowThruContracts = _contractsRepository.GetFloThruSummaryByAccountExecutiveId(_session.CurrentUser.Id),
                FullySourcedContracts = _contractsRepository.GetFullySourcedSummaryByAccountExecutiveId(_session.CurrentUser.Id),
                Jobs = _jobsRepository.GetSummaryCountsByAccountExecutiveId(_session.CurrentUser.Id),
                Timesheets = _timesheetsRepository.GetTimesheetSummaryByAccountExecutiveId(_session.CurrentUser.Id)
            };

            return dashboardInfo;
        }
    }
}
