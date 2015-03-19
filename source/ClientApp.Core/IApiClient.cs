using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface IApiClient
    {
        Task<ValidationResult> Authenticate(string username, string password, string caller = null);
        Task Deauthenticate(string caller = null);

        Task<TResult> Get<TResult>(object parameters = null, string caller = null);

        Task Post(object data, bool authenticate = true, string caller = null);
        Task<TResult> Post<TResult>(object content, bool authenticate = true, string caller = null);
    }
}
