using System;
namespace ClientApp.Core
{
    public interface IApiClient
    {
        System.Threading.Tasks.Task<ValidationResult> Authenticate(string username, string password, string caller = null);
        System.Threading.Tasks.Task Deauthenticate(string caller = null);
        System.Threading.Tasks.Task<TResult> Get<TResult>(object parameters, string caller = null);
        System.Threading.Tasks.Task<TResult> Get<TResult>(object values, System.Threading.CancellationToken cancellationToken, string caller = null);
        System.Threading.Tasks.Task<TResult> Get<TResult>(string caller = null);
        System.Threading.Tasks.Task Post(object data, string caller = null);
        System.Threading.Tasks.Task Post(object data, System.Threading.CancellationToken cancellationToken, string caller = null);
    }
}
