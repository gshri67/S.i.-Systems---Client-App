using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface IApiClient
    {
        Task<ValidationResult> Authenticate(string username, string password, string caller = null);
        Task Deauthenticate(string caller = null);
        Task<TResult> Get<TResult>(object parameters, string caller = null);
        Task<TResult> Get<TResult>(string caller = null);
        Task Post(HttpContent data, string caller = null);
        Task<TResult> PostUnauthenticated<TResult>(HttpContent content, string caller = null);
    }
}
