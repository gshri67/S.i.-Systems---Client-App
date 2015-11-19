using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using System.Net.Http;

namespace Shared.Core
{
    public interface IMatchGuideApi
    {
        Task<ValidationResult> Login(string username, string password);

        Task Logout();

        Task<Consultant> GetConsultant(int id);

        Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query);

        Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query);

        Task Submit(ContractProposal proposal);

        Task SendMessage(ConsultantMessage message);

        Task<Eula> GetMostRecentEula();

        Task<ClientAccountDetails> GetClientDetails();

        Task<ResetPasswordResult> ResetPassword(string emailAddress);
        
        Task<IEnumerable<PayPeriod>> GetPayPeriods();

		Task<IEnumerable<string>> GetProjectCodes();

		Task<IEnumerable<PayRate>> GetPayRates(int contractId);

		Task<IEnumerable<Remittance>> GetRemittances();

        Task<IEnumerable<DirectReport>> GetTimesheetApproversByTimesheetId( int clientId );
        
        Task<ConsultantDetails> GetCurrentUserConsultantDetails();

        Task<HttpResponseMessage> GetPDF(string docNumber);
        
        Task<Timesheet> SaveTimesheet(Timesheet timesheet);
        
        Task<Timesheet> SubmitTimesheet(Timesheet timesheet);

		Task<DashboardSummary> getDashboardInfo();

        Task<IEnumerable<Job>> GetJobs();
        
        Task<IEnumerable<ConsultantContractSummary>> GetContracts();
        Task<ConsultantContract> GetContractWithId( int id );
        
        Task<JobDetails> GetJobDetails(int id);
        
        Task<ConsultantContract> GetContractDetails(int id);
    }
}
