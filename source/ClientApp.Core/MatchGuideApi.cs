using ClientApp.Core.HttpAttributes;
using SiSystems.ClientApp.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace ClientApp.Core
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
    }

    [Api(Settings.MatchGuideApiAddress)]
    public class MatchGuideApi : IMatchGuideApi
    {
        private readonly ApiClient<MatchGuideApi> _client;

        public MatchGuideApi(ApiClient<MatchGuideApi> client)
        {
            this._client = client;
        }

        [HttpPost("login")]
        public async Task<ValidationResult> Login(string username, string password)
        {
            return await this._client.Authenticate(username, password);
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await this._client.Deauthenticate();
        }

        [HttpGet("consultants/alumni/{id}")]
        public async Task<Consultant> GetConsultant(int id)
        {
            return await this._client.Get<Consultant>(new { id });
        }

        [HttpGet("consultants/alumni")]
        public async Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query)
        {
            return await this._client.Get<ConsultantGroup[]>(new { query });
        }

        [HttpGet("consultants/active")]
        public async Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query)
        {
            return await this._client.Get<ConsultantGroup[]>(new { query });
        }

        [HttpGet("eula")]
        public async Task<Eula> GetMostRecentEula()
        {
            return await this._client.Get<Eula>();
        }

        [HttpPost("contractproposal")]
        public Task Submit(ContractProposal proposal)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(proposal), System.Text.Encoding.UTF8, "application/json");
            return this._client.Post(content);
        }

        [HttpPost("consultantmessages")]
        public Task SendMessage(ConsultantMessage message)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(message), System.Text.Encoding.UTF8, "application/json");
            return this._client.Post(content);
        }

        [HttpGet("clientdetails")]
        public Task<ClientAccountDetails> GetClientDetails()
        {
            return this._client.Get<ClientAccountDetails>();
        }

        [HttpPost("forgotpassword")]
        public Task<ResetPasswordResult> ResetPassword(string emailAddress)
        {
            // Web API expects form url encoded payload with no key
            var payload = new FormUrlEncodedContent(new Dictionary<string, string> { { string.Empty, emailAddress } });
            return this._client.PostUnauthenticated<ResetPasswordResult>(payload);
        }
    }
}
