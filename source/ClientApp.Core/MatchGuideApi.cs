using ClientApp.Core.HttpAttributes;
using SiSystems.ClientApp.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace ClientApp.Core
{
    [Api(Settings.MatchGuideApiAddress)]
    public interface IMatchGuideApi
    {
        [HttpPost("login")]
        Task<ValidationResult> Login(string username, string password);

        [HttpPost("logout")]
        Task Logout();

        [HttpGet("consultants/{id}")]
        Task<Consultant> GetConsultant(int id);

        [HttpGet("consultants/alumni")]
        Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query);

        [HttpGet("consultants/active")]
        Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query);

        [HttpPost("contractproposal")]
        Task Submit(ContractProposal proposal);

        [HttpPost("consultantmessages")]
        Task SendMessage(ConsultantMessage message);

        [HttpGet("eula")]
        Task<Eula> GetMostRecentEula();

        [HttpGet("clientdetails")]
        Task<ClientAccountDetails> GetClientDetails();

        [HttpPost("forgotpassword")]
        Task<ResetPasswordResult> ResetPassword(string emailAddress);
    }

    public class MatchGuideApi : IMatchGuideApi
    {
        private readonly ApiClient<IMatchGuideApi> _client;

        public MatchGuideApi(ApiClient<IMatchGuideApi> client)
        {
            this._client = client;
        }

        public async Task<ValidationResult> Login(string username, string password)
        {
            return await this._client.Authenticate(username, password);
        }

        public async Task Logout()
        {
            await this._client.Deauthenticate();
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            return await this._client.Get<Consultant>(new { id });
        }

        public async Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query)
        {
            return await this._client.Get<ConsultantGroup[]>(new { query });
        }

        public async Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query)
        {
            return await this._client.Get<ConsultantGroup[]>(new { query });
        }

        public async Task<Eula> GetMostRecentEula()
        {
            return await this._client.Get<Eula>();
        }

        public Task Submit(ContractProposal proposal)
        {
            return this._client.Post(proposal);
        }

        public Task SendMessage(ConsultantMessage message)
        {
            return this._client.Post(message);
        }

        public Task<ClientAccountDetails> GetClientDetails()
        {
            return this._client.Get<ClientAccountDetails>();
        }

        public Task<ResetPasswordResult> ResetPassword(string emailAddress)
        {
            // Web API expects form url encoded payload with no key
            var payload = new FormUrlEncodedContent(new Dictionary<string, string> { { string.Empty, emailAddress } });
            return this._client.PostUnauthenticated<ResetPasswordResult>(payload);
        }
    }
}
