using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.SharedModels.Contract_Creation;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class ContractCreationSupportService
    {
        private readonly IInternalEmployeesRepository _internalEmployeesRepository;
        private readonly IPickListValuesRepository _pickListValuesRepository;
        private readonly ISessionContext _session;

        public ContractCreationSupportService(ISessionContext session, IInternalEmployeesRepository repo, IPickListValuesRepository pickListValuesRepository)
        {
            _session = session;
            _internalEmployeesRepository = repo;
            _pickListValuesRepository = pickListValuesRepository;
        }

        private IEnumerable<InternalEmployee> ColleaguesForCurrentUser()
        {
            var accountExecutives = _internalEmployeesRepository.GetAccountExecutivesThatShareBranchWithUserId(_session.CurrentUser.Id);
            
            return accountExecutives.OrderBy(employee => employee.FirstName).ThenBy(employee => employee.LastName);
        }

        private IEnumerable<string> InvoiceFormats()
        {
            return _pickListValuesRepository.InvoiceFormatOptions();
        }

        private IEnumerable<string> InvoiceFrequencies()
        {
            return _pickListValuesRepository.InvoiceFrequencyOptions();
        }

        private IEnumerable<string> ContractLimitaionTypes()
        {
            return _pickListValuesRepository.ContractLimitationOptions();
        }

        private IEnumerable<string> ContractPaymentPlans()
        {
            return _pickListValuesRepository.ContractPaymentPlans();
        }

        private IEnumerable<string> RateTermTypes()
        {
            return _pickListValuesRepository.RateTermTypes();
        }

        public ContractCreationOptions GetContractOptionsForMainForm()
        {
            return new ContractCreationOptions
            {
                Colleagues = ColleaguesForCurrentUser(),
                InvoiceFormatOptions = InvoiceFormats(),
                InvoiceFrequencyOptions = InvoiceFrequencies(),
                LimitationOfContractTypeOptions = ContractLimitaionTypes(),
                PaymentPlanOptions = ContractPaymentPlans()
            };
        }

        public RateOptions GetContractOptionsForRates()
        {
            return new RateOptions
            {
                RateTypes = RateTermTypes()
            };
        }
    }
}
