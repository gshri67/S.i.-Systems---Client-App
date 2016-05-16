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
        private readonly IUserContactRepository _usersRepo;
        private readonly ISessionContext _session;

        public ContractCreationSupportService(ISessionContext session, IInternalEmployeesRepository repo, IPickListValuesRepository pickListValuesRepository, IJobsRepository jobsRepository, IUserContactRepository usersRepo)
        {
            _session = session;
            _internalEmployeesRepository = repo;
            _jobsRepository = jobsRepository;
            _pickListValuesRepository = pickListValuesRepository;
            _usersRepo = usersRepo;
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

        private IEnumerable<string> Branches()
        {
            return _pickListValuesRepository.BranchTypeOptions();
        }

        private IEnumerable<InternalEmployee> ColleaguesForCurrentUser()
        {
            var accountExecutives = _internalEmployeesRepository.GetAccountExecutivesThatShareBranchWithUserId(_session.CurrentUser.Id);
            
            return accountExecutives.OrderBy(employee => employee.FirstName).ThenBy(employee => employee.LastName);
        }

        private IEnumerable<InternalEmployee> ColleaguesForBranch(int branch)
        {
            var accountExecutives = _internalEmployeesRepository.GetAccountExecutivesWithBranch(branch);
            
            if( accountExecutives != null )
                accountExecutives = accountExecutives.OrderBy(employee => employee.FirstName).ThenBy(employee => employee.LastName);

            return accountExecutives;
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

        private IEnumerable<InternalEmployee> ClientContacts(int companyId)
        {
            return _internalEmployeesRepository.GetClientContactsWithCompanyId(companyId);
        }
        private IEnumerable<InternalEmployee> BillingContacts(int companyId)
        {
            return _internalEmployeesRepository.GetBillingContactsWithCompanyId(companyId);
        }
        private IEnumerable<InternalEmployee> DirectReports(int companyId)
        {
            return _internalEmployeesRepository.GetDirectReportsWithCompanyId(companyId);
        }
        private IEnumerable<InternalEmployee> InvoiceRecipients(int agreementId)
        {
            return _internalEmployeesRepository.GetInvoiceRecipientsWithAgreementId(agreementId);
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
                BranchOptions = Branches()
                
            };
        }

        public IEnumerable<InternalEmployee> GetColleaguesWithBranch(int branch)
        {
            return ColleaguesForBranch(branch);
        }

        public ContractCreationOptions_Sending GetContractOptionsForSendingForm(int jobId)
        {
            int companyId = _jobsRepository.GetCompanyIdWithJobId(jobId);

            return new ContractCreationOptions_Sending
            {
                ClientContactOptions = ClientContacts(companyId).ToList(),
                DirectReportOptions = DirectReports(companyId).ToList(),
                BillingContactOptions = BillingContacts(companyId).ToList(),
                InvoiceRecipientOptions = InvoiceRecipients(jobId).ToList()
            };
        }

        public RateOptions GetContractOptionsForRates()
        {
            return new RateOptions
            {
                RateTypes = RateTermTypes()
            };
        }

        public string GetEmailSubject( int jobId )
        {
            Job job = _jobsRepository.GetJobWithJobId(jobId);
            return string.Format("Agreement between S.i. Systems Partnership and {0} (Contract {1})", job.ClientName, jobId);
        }

        public string GetEmailBody( int jobId, int candidateId )
        {
            Job job = _jobsRepository.GetJobWithJobId(jobId);
            UserContact candidate = _usersRepo.GetUserContactById(candidateId);

            if( job != null && candidate != null )
                return string.Format("Dear  {0},\n\nYou are invited by S.i. Systems Partnership (\"SI\") to log on to its secure web portal as the representative and agent of {1} (the \"Consultant\"). Once you have logged into the web portal, you will be asked to review, and if the terms are acceptable, accept the form of agreement between SI and the Consultant, pursuant to which, the Consultant will provide services to SI’s client {2}.\n\nClick one of the below links to log into your account and accept the agreement.\n\nClick here for mobile site.\n\nClick here for desktop site.\n\nPlease note that you can only access and view the form of agreement using the above links. Once you have accepted the agreement on behalf of the Consultant, you will receive a confirmation via email at this email address that the agreement has been entered into and a PDF copy of the agreement’s terms and conditions will be stored in your web portal.\n\nPlease contact me if you have any questions about the terms and conditions of the form of the agreement.\n\nRegards,\n\n", candidate.FullName, job.ClientName, "As per declaration of partnership" );
            return string.Empty;
        }
    }
}
