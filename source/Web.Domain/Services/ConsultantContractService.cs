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
        private readonly IContractorRepository _contractorRepository;
        private readonly IUserContactRepository _userContactRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ISessionContext _session;

        public ConsultantContractService(IConsultantContractRepository consultantContractRepository, IContractorRepository contractorRepository, IUserContactRepository userContactRepository,IDateTimeService dateTimeService, ISessionContext session)
        {
            _consultantContractRepository = consultantContractRepository;
            _contractorRepository = contractorRepository;
            _userContactRepository = userContactRepository;
            _dateTimeService = dateTimeService;
            _session = session;
        }

        public ContractStatusType ContractStatusTypeForStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            if(_dateTimeService.DateIsWithinNextThirtyDays(startDate))
                return ContractStatusType.Starting;

            if (_dateTimeService.DateIsWithinNextThirtyDays(endDate))
                return ContractStatusType.Ending;
            
            //todo: Is Active what we would actually want these to show as? Should there be another status? Future? Past?
            return ContractStatusType.Active;
        }

        public IEnumerable<ConsultantContractSummary> GetContractSummariesForAccountExecutive() 
        {
            var contracts = _consultantContractRepository.GetContractSummaryByAccountExecutiveId(_session.CurrentUser.Id);

            foreach (var contract in contracts)
            {
                contract.StatusType = ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);
            }

            return contracts;
        }

        private Contractor GetContractorByContractId(int id)
        {
            var contractorId = _contractorRepository.GetContractorIdByContractId(id);
            var contractor = _contractorRepository.GetContractorById(contractorId);
            contractor.ContactInformation = _userContactRepository.GetUserContactById(contractorId);
            contractor.Contracts = _consultantContractRepository.GetContractsByContractorId(contractorId);

            return contractor;
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            var details = _consultantContractRepository.GetContractDetailsById(id);
            
            details.StatusType = ContractStatusTypeForStartDateAndEndDate(details.StartDate, details.EndDate);

            details.Contractor = GetContractorByContractId(id);
            details.DirectReport = _userContactRepository.GetDirectReportByAgreementId(details.ContractId);
            details.ClientContact = _userContactRepository.GetClientContactByAgreementId(details.ContractId);
            details.BillingContact = _userContactRepository.GetBillingContactByAgreementId(details.ContractId);

            details.GrossMargin = details.BillRate - details.PayRate;
            details.Markup = (details.BillRate - details.PayRate)/details.PayRate;
            details.Margin = (details.BillRate - details.PayRate) / details.BillRate;

            AssertCurrentUserHasAccessToContractDetails(details);

            return details;
        }

        /// <summary>
        /// Validate that the current user is the Account Executive for the Contract Details being requested
        /// </summary>
        private void AssertCurrentUserHasAccessToContractDetails(ConsultantContract details)
        {
            //todo: work out the permissions regarding which AE has access to what details
            if(false)
                throw new UnauthorizedAccessException();
        }
    }
}
