using ClientApp.Core.HttpAttributes;
using SiSystems.ClientApp.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ClientApp.Core
{
    public interface IMatchGuideApi
    {
        Task<ValidationResult> Login(string username, string password);

        Task Logout();

        Task<Consultant> GetConsultant(int id);

        Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query);

        Task Submit(ContractProposal proposal);

        Task SendMessage(ConsultantMessage message);

        Task<Eula> GetMostRecentEula();
        Task<ClientAccountDetails> GetClientDetails();
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
        public async Task<IEnumerable<ConsultantGroup>> GetConsultantGroups(string query)
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
            return this._client.Post(proposal);
        }

        [HttpPost("consultantmessages")]
        public Task SendMessage(ConsultantMessage message)
        {
            return this._client.Post(message);
        }

        [HttpGet("clientdetails")]
        public Task<ClientAccountDetails> GetClientDetails()
        {
            return this._client.Get<ClientAccountDetails>();
        }
    }
}
