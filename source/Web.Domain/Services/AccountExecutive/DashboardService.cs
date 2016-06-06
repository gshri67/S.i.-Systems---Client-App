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
            var id = _session.CurrentUser.Id;
            var dashboardInfo = new DashboardSummary
            {
				UserName = _session.CurrentUser.FullName,
                FlowThruContracts = new ContractSummarySet
                    {
                        Current = _contractsRepository.ActiveFloThruContractsForAccountExecutive(id).Count(),
                        Starting = _contractsRepository.StartingFloThruContractsForAccountExecutive(id).Count(),
                        Ending = _contractsRepository.EndingFloThruContractsForAccountExecutive(id).Count()
                    },
                FullySourcedContracts = new ContractSummarySet
                    {
                        Current = _contractsRepository.ActiveFullySourcedContractsForAccountExecutive(id).Count(),
                        Starting = _contractsRepository.StartingFullySourcedContractsForAccountExecutive(id).Count(),
                        Ending = _contractsRepository.EndingFullySourcedContractsForAccountExecutive(id).Count()
                    },
                Jobs = _jobsRepository.GetSummaryCountsByAccountExecutiveId(_session.CurrentUser.Id),
                Timesheets = _timesheetsRepository.GetTimesheetSummaryByAccountExecutiveId(_session.CurrentUser.Id)
            };

            return dashboardInfo;
        }
    }
}
