using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class ConsultantContractService
    {
        private readonly IConsultantContractRepository _consultantContractRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ISessionContext _session;

        public ConsultantContractService(IConsultantContractRepository consultantContractRepository, IDateTimeService dateTimeService, ISessionContext session)
        {
            _consultantContractRepository = consultantContractRepository;
            _dateTimeService = dateTimeService;
            _session = session;
        }

        public ContractStatusType ContractStatusTypeForConsultantContract(ConsultantContractSummary contract)
        {
            if(_dateTimeService.DateIsWithinNextThirtyDays(contract.StartDate))
                return ContractStatusType.Starting;

            if (_dateTimeService.DateIsWithinNextThirtyDays(contract.EndDate))
                return ContractStatusType.Ending;
            
            //todo: Is Active what we would actually want these to show as? Should there be another status? Future? Past?
            return ContractStatusType.Active;
        }

        public IEnumerable<ConsultantContractSummary> GetContractSummariesForAccountExecutive() 
        {
            var repoContracts = _consultantContractRepository.GetContractSummaryByAccountExecutiveId(_session.CurrentUser.Id);

            foreach (var contract in repoContracts)
            {
                contract.StatusType = ContractStatusTypeForConsultantContract(contract);
            }

            return repoContracts;
        }
    }
}
