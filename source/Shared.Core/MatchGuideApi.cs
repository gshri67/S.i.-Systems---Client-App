﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Core.HttpAttributes;
using Shared.Core.Platform;
using SiSystems.SharedModels;
using Xamarin;

namespace Shared.Core
{
    [Api(Settings.MatchGuideApiAddress)]
    public class MatchGuideApi : ApiClient, IMatchGuideApi
    {
        private readonly ITokenStore _tokenStore;

		public MatchGuideApi(ITokenStore tokenStore, IDefaultStore defaultStore, IActivityManager activityManager, IErrorSource errorSource, IHttpMessageHandlerFactory handlerFactory) 
			: base(tokenStore, defaultStore, activityManager, errorSource, handlerFactory)
        {
            this._tokenStore = tokenStore;
        }

        [HttpPost("login")]
        public async Task<ValidationResult> Login(string username, string password)
        {
            try
            {
                _defaultStore.Username = username;

                var response = await ExecuteWithDefaultClient(new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "username", WebUtility.HtmlEncode (username) },
                        { "password", WebUtility.HtmlEncode (password) },
                        { "grant_type", "password" }
                    }));
                string json = null;
                if (response.Content != null)
                {
                    json = await response.Content.ReadAsStringAsync();
                }
                var validationResult = new ValidationResult();
                if (response.IsSuccessStatusCode)
                {
                    var token = JsonConvert.DeserializeObject<OAuthToken>(json);
                    _tokenStore.SaveToken(token);

//                    Insights.Identify(token.Username, new Dictionary<string, string>
//                    {
//                        { "Token Expires At", token.ExpiresAt },
//                        { "Token Expires In", token.ExpiresIn.ToString() },
//                        { "Token Issued At", token.IssuedAt }
//                    });

                    validationResult.IsValid = true;
                }
                else
                {
                    if (json != null && json[0] == '{')
                    {
                        var apiMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(json);
                        validationResult.Message = apiMessage.ErrorDescription;
                    }
                    else
                    {
                        validationResult.Message = json;
                    }
                }

                return validationResult;
            }
            catch (Exception e)
            {
                Insights.Report(e);
                return new ValidationResult { IsValid = false, Message = e.Message };
            }
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            this._tokenStore.DeleteDeviceToken();
            await ExecuteWithDefaultClient();
        }

        [HttpGet("consultants/{id}")]
        public async Task<Consultant> GetConsultant(int id)
        {
            return await ExecuteWithDefaultClient<Consultant>(new { id });
        }

        [HttpGet("consultants/alumni")]
        public async Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query)
        {
            return await ExecuteWithDefaultClient<ConsultantGroup[]>(new { query });
        }

        [HttpGet("consultants/active")]
        public async Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query)
        {
            return await ExecuteWithDefaultClient<ConsultantGroup[]>(new { query });
        }

        [HttpGet("eula")]
        public async Task<Eula> GetMostRecentEula()
        {
            return await ExecuteWithDefaultClient<Eula>();
        }

        [HttpPost("contractproposal")]
        public Task Submit(ContractProposal proposal)
        {
            return ExecuteWithDefaultClient(proposal);
        }

        [HttpPost("consultantmessages")]
        public Task SendMessage(ConsultantMessage message)
        {
            return ExecuteWithDefaultClient(message);
        }

        [HttpGet("clientdetails")]
        public Task<ClientAccountDetails> GetClientDetails()
        {
            return ExecuteWithDefaultClient<ClientAccountDetails>();
        }

        [HttpPost("forgotpassword")]
        public Task<ResetPasswordResult> ResetPassword(string emailAddress)
        {
            // Web API expects form url encoded payload with no key
            var payload = new FormUrlEncodedContent(new Dictionary<string, string> { { string.Empty, emailAddress } });
            return ExecuteWithDefaultClient<ResetPasswordResult>(payload);
        }

        [HttpGet("PayPeriods")]
        public async Task<IEnumerable<PayPeriod>> GetPayPeriods()
        {
            return await ExecuteWithDefaultClient<PayPeriod[]>();
        }
        
        [HttpGet("PayPeriods/Summaries")]
        public async Task<IEnumerable<PayPeriod>> GetPayPeriodSummaries()
        {
            return await ExecuteWithDefaultClient<PayPeriod[]>();
        }
        
        [HttpGet("Remittances")]
		public async Task<IEnumerable<Remittance>> GetRemittances()
		{
		    return await ExecuteWithDefaultClient<IEnumerable<Remittance>>();
		}

        [HttpGet("TimesheetApprovers/{id}")]
        public async Task<IEnumerable<DirectReport>> GetTimesheetApproversByAgreementId(int agreementId)
        {
            return await ExecuteWithDefaultClient<IEnumerable<DirectReport>>(new { id = agreementId });
        }

        [HttpGet("ConsultantDetails")]
        public async Task<ConsultantDetails> GetCurrentUserConsultantDetails()
        {
            return await ExecuteWithDefaultClient<ConsultantDetails>();
        }

        [HttpPost("Remittances/pdf/remittanceVar")]
        public async Task<Stream> GetPDF(Remittance rm)
        {
            HttpResponseMessage response = await ExecuteWithStreamingClient(rm);
            try
            {
                if (response != null)
                    return await response.Content.ReadAsStreamAsync();
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost("Timesheets/Details")]
        public async Task<Timesheet> PopulateTimeEntries(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<Timesheet>(timesheet);
        }

        [HttpPost("Timesheets/Save")]
        public async Task<Timesheet> SaveTimesheet(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<Timesheet>(timesheet);
        }
			
        [HttpPost("Timesheets/Submit")]
        public async Task<Timesheet> SubmitTimesheet(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<Timesheet>(timesheet);
        }

        [HttpPost("Timesheets/Withdraw")]
        public async Task<Timesheet> WithdrawTimesheet(int timesheetId, string cancelReason )
        {
            return await ExecuteWithDefaultClient<Timesheet>(new {timesheetId, cancelReason} );
        }

		[HttpGet("Dashboard")]
		public async Task<DashboardSummary> getDashboardInfo()
		{
			return await ExecuteWithDefaultClient<DashboardSummary>();
		}

        [HttpGet("Jobs/Summaries")]
        public async Task<IEnumerable<JobSummary>> GetJobSummaries()
        {
            return await ExecuteWithDefaultClient<IEnumerable<JobSummary>>();
        }

        [HttpGet("Jobs/Client/{id}")]
        public async Task<IEnumerable<Job>> GetJobsWithClientID( int id )
        {
            return await ExecuteWithDefaultClient<IEnumerable<Job>>(new {id} );
        }

        [HttpGet("ConsultantContracts")]
        public async Task<IEnumerable<ConsultantContractSummary>> GetContracts()
        {
            return await ExecuteWithDefaultClient<IEnumerable<ConsultantContractSummary>>();
        }
         
        [HttpGet("ConsultantContracts/Details/{id}")]
        public async Task<ConsultantContract> GetContractDetailsById(int id)
        {
            return await ExecuteWithDefaultClient<ConsultantContract>(new {id});
        }

        [HttpGet("Jobs/{id}")]
        public async Task<Job> GetJobWithJobId(int id)
        {
            return await ExecuteWithDefaultClient<Job>(new { id });
        }

        [HttpGet("Contractors/Job/{id}/Status/{status}")]
        public async Task<IEnumerable<Contractor>> GetContractorsWithJobIdAndStatus(int id, JobStatus status )
        {
            return await ExecuteWithDefaultClient<IEnumerable<Contractor>>(new { id, status });
        }

        [HttpGet("Contractors/{id}")]
        public async Task<Contractor> GetContractorById(int id)
        {
            return await ExecuteWithDefaultClient<Contractor>(new { id });
        }

        [HttpGet("UserContact/{id}")]
        public async Task<UserContact> GetUserContactById(int id)
        {
            return await ExecuteWithDefaultClient<UserContact>(new { id });
        }

        [HttpGet("ContractorRate/Job/{id}/Status/{status}")]
        public async Task<IEnumerable<ContractorRateSummary>> GetContractorRateSummaryWithJobIdAndStatus(int id, JobStatus status )
        {
            return await ExecuteWithDefaultClient<IEnumerable<ContractorRateSummary>>(new { id, status });
        }

        [HttpGet("Timesheets/Reporting/Summary")]
        public async Task<TimesheetSummarySet> GetTimesheetSummary()
        {
            return await ExecuteWithDefaultClient<TimesheetSummarySet>();
        }

        [HttpGet("Timesheets/Reporting/Details/Status/{status}")]
        public async Task<IEnumerable<TimesheetDetails>> GetTimesheetDetails( MatchGuideConstants.TimesheetStatus ts)
        {
            return await ExecuteWithDefaultClient<IEnumerable<TimesheetDetails>>(new { status = ts.ToString() });
        }

        [HttpGet("Timesheets/Reporting/Contact/{id}")]
        public async Task<TimesheetContact> GetTimesheetContactById(int id)
        {
            return await ExecuteWithDefaultClient<TimesheetContact>(new { id });
        }

        [HttpGet("Timesheets/Reporting/Contact/Open/{id}")]
        public async Task<TimesheetContact> GetOpenTimesheetContactByAgreementId(int id)
        {
            return await ExecuteWithDefaultClient<TimesheetContact>(new { id });
        }

        [HttpGet("Contractors")]
        public async Task<IEnumerable<Contractor>> GetContractors()
        {
            return await ExecuteWithDefaultClient<IEnumerable<Contractor>>();
        }

        [HttpGet("UserContact")]
        public async Task<IEnumerable<UserContact>> GetClientContacts()
        {
            return await ExecuteWithDefaultClient<IEnumerable<UserContact>>();
        }

        [HttpGet("UserContact/Filter/{filter}")]
        public async Task<IEnumerable<UserContact>> GetClientContactsWithFilter( string filter )
        {
            return await ExecuteWithDefaultClient<IEnumerable<UserContact>>( new {filter} );
        }

        [HttpPost("PayRates/TimesheetSupport")]
        public async Task<TimesheetSupport> GetTimesheetSupportForTimesheet(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<TimesheetSupport>(timesheet);
        }

    }
}
