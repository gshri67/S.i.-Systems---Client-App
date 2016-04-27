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
        private readonly IJobsRepository _jobsRepository;
        private readonly IPickListValuesRepository _pickListValuesRepository;
        private readonly ISessionContext _session;

        public ContractCreationSupportService(ISessionContext session, IInternalEmployeesRepository repo, IPickListValuesRepository pickListValuesRepository, IJobsRepository jobsRepository)
        {
            _session = session;
            _internalEmployeesRepository = repo;
            _jobsRepository = jobsRepository;
            _pickListValuesRepository = pickListValuesRepository;
        }

        private IEnumerable<string> TimeFactors()
        {
            return _pickListValuesRepository.TimeFactorOptions();
        }

        private IEnumerable<string> DaysCancellation()
        {
            return _pickListValuesRepository.DaysCancellationOptions();
        }

        private IEnumerable<string> LimitationExpenses()
        {
            return _pickListValuesRepository.LimitationExpenseOptions();
        }

        private IEnumerable<string> LimitationOfContractTypes()
        {
            return _pickListValuesRepository.LimitationOfContractTypeOptions();
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

        private IEnumerable<string> ContractPaymentPlans()
        {
            return _pickListValuesRepository.ContractPaymentPlans();
        }

        private IEnumerable<string> RateTermTypes()
        {
            return _pickListValuesRepository.RateTermTypes();
        }

        private IEnumerable<InternalEmployee> ClientContacts( int companyId )
        {
            return _internalEmployeesRepository.GetClientContactsWithCompanyId(companyId);
        }


        public ContractCreationOptions GetContractOptionsForMainForm( int jobId )
        {
            int companyId = _jobsRepository.GetCompanyIdWithJobId(jobId);

            return new ContractCreationOptions
            {
                TimeFactorOptions = TimeFactors(),
                DaysCancellationOptions = DaysCancellation(),
                //LimitationExpenseOptions = LimitationExpenses(),
                LimitationOfContractTypeOptions = LimitationOfContractTypes(),
                Colleagues = ColleaguesForCurrentUser(),
                InvoiceFormatOptions = InvoiceFormats(),
                InvoiceFrequencyOptions = InvoiceFrequencies(),
                PaymentPlanOptions = ContractPaymentPlans(),
                
            };
        }


        public ContractCreationOptions_Sending GetContractOptionsForSendingForm(int jobId)
        {
            int companyId = _jobsRepository.GetCompanyIdWithJobId(jobId);

            return new ContractCreationOptions_Sending
            {
                ClientContactOptions = ClientContacts(companyId).ToList(),
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
