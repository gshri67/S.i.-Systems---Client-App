﻿using System;
using System.Collections.Generic;
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

        public MatchGuideApi(ITokenStore tokenStore, IActivityManager activityManager, IErrorSource errorSource, IHttpMessageHandlerFactory handlerFactory) 
            : base(tokenStore, activityManager, errorSource, handlerFactory)
        {
            this._tokenStore = tokenStore;
        }

        [HttpPost("login")]
        public async Task<ValidationResult> Login(string username, string password)
        {
            try
            {
                this.Username = username;

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
                    _tokenStore.SaveUserName(token.Username);

                    Insights.Identify(token.Username, new Dictionary<string, string>
                    {
                        { "Token Expires At", token.ExpiresAt },
                        { "Token Expires In", token.ExpiresIn.ToString() },
                        { "Token Issued At", token.IssuedAt }
                    });

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

		[HttpGet("Timesheets/ProjectCodes")]
		public async Task<IEnumerable<string>> GetProjectCodes()
		{

			List<String> list = new List<string> ();
			list.Add ("PC123");
			list.Add ("PC124");
			list.Add ("PC125");
			list.Add ("PC126");
			list.Add ("PC127");
			list.Add ("PC128");

			IEnumerable<string> enumerableList = list;
			return list;

			//return await ExecuteWithDefaultClient<string[]>();
		}

		[HttpGet("PayRates/{id}")]
		public async Task<IEnumerable<PayRate>> GetPayRates(int id)
		{
            return await ExecuteWithDefaultClient<IEnumerable<PayRate>>(new { id });
		}

        [HttpGet("Remittances")]
		public async Task<IEnumerable<Remittance>> GetRemittances()
		{
		    return await ExecuteWithDefaultClient<IEnumerable<Remittance>>();
		}

        [HttpGet("TimesheetApprovers/{id}")]
        public async Task<IEnumerable<DirectReport>> GetTimesheetApproversByTimesheetId(int clientId)
        {
            return await ExecuteWithDefaultClient<IEnumerable<DirectReport>>(new { id = clientId });
        }

        [HttpGet("ConsultantDetails")]
        public async Task<ConsultantDetails> GetCurrentUserConsultantDetails()
        {
            return await ExecuteWithDefaultClient<ConsultantDetails>();
        }

        [HttpGet("Remittances/pdf/{docNumber}")]
        public async Task<HttpResponseMessage> GetPDF(string docNumber) 
        {
            return await ExecuteWithDefaultClient<HttpResponseMessage>(new { docNumber });
        }

        [HttpPost("Timesheets")]
        public async Task<Timesheet> SaveTimesheet(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<Timesheet>(timesheet);
        }
			
        [HttpPost("Timesheets/Submit")]
        public async Task<Timesheet> SubmitTimesheet(Timesheet timesheet)
        {
            return await ExecuteWithDefaultClient<Timesheet>(timesheet);
        }

		[HttpGet("Dashboard")]
		public async Task<DashboardSummary> getDashboardInfo()
		{
			return await ExecuteWithDefaultClient<DashboardSummary>();
		}

        [HttpGet("Job")]
        public async Task<IEnumerable<Job>> getJobs()
        {
            return await ExecuteWithDefaultClient<IEnumerable<Job>>();
        }

        [HttpGet("ConsultantContract")]
        public async Task<IEnumerable<ConsultantContract>> getContracts()
        {
            return await ExecuteWithDefaultClient<IEnumerable<ConsultantContract>>();
        }
    }
}
