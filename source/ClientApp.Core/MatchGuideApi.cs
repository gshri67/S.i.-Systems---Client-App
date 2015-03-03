using ClientApp.Core.HttpAttributes;
using SiSystems.ClientApp.SharedModels;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface IMatchGuideApi
    {
        Task<bool> Login(string username, string password);

        Task<Consultant> GetConsultant(int id);
    }

#if LOCAL
    [Api("http://clientapi.local:50021/api/")]
#elif DEV
    [Api("https://clientapidev.azurewebsites.net/api/")]
#elif TEST
    [Api("https://clientapitest.azurewebsites.net/api/")]
#elif PROD
    [Api("https://clientapi.azurewebsites.net/api/")]
#endif
    public class MatchGuideApi : IMatchGuideApi
    {
        private readonly ApiClient<MatchGuideApi> _client;

        public MatchGuideApi(ApiClient<MatchGuideApi> client)
        {
            this._client = client;
        }

        [HttpPost("login")]
        public async Task<bool> Login(string username, string password)
        {
            return await this._client.Authenticate(username, password);
        }

        [HttpGet("consultant/{id}")]
        public async Task<Consultant> GetConsultant(int id)
        {
            return await this._client.Get<Consultant>(new { id });
        }
    }
}
