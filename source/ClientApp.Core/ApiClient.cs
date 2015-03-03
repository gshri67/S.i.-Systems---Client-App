using ClientApp.Core.HttpAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class ApiClient<TApi>
    {
        private readonly ITokenStore _tokenStore;

        private readonly IActivityManager _activityManager;

        private readonly HttpMessageHandler _handler;

        private readonly Uri _baseAddr;

        private OAuthToken _token;

        public ApiClient(ITokenStore tokenStore, IActivityManager activityManger, HttpMessageHandler handler)
        {
            this._tokenStore = tokenStore;
            this._handler = handler;
            this._baseAddr = new Uri(typeof(TApi).GetTypeInfo().GetCustomAttribute<ApiAttribute>().BaseUrl);
        }

        public async Task<bool> Authenticate(string username, string password, [CallerMemberName] string caller = null)
        {
            var httpClient = new HttpClient(this._handler) { BaseAddress = _baseAddr };
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "username", WebUtility.HtmlEncode (username) },
                        { "password", WebUtility.HtmlEncode (password) },
                        { "grant_type", "password" }
                    });

            var request = new HttpRequestMessage(HttpMethod.Get, GetUriFromAction(caller, null))
            {
                Content = content
            };

            var authenticationResponse = await httpClient.SendAsync(request);

            authenticationResponse.EnsureSuccessStatusCode();

            var tokenJsonString = await authenticationResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<OAuthToken>(tokenJsonString);

            this._token = _tokenStore.Store(token);

            return true;
        }

        public async Task<TResult> Get<TResult>(object parameters, [CallerMemberName] string caller = null)
        {
            return await Get<TResult>(parameters, CancellationToken.None, caller);
        }

        public async Task<TResult> Get<TResult>(object values, CancellationToken cancellationToken, [CallerMemberName] string caller = null)
        {
            var response = await ExecuteWithAuthenticatedClient(async httpClient => await httpClient.GetAsync(GetUriFromAction(caller, values), HttpCompletionOption.ResponseHeadersRead, cancellationToken), cancellationToken);

            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TResult>(jsonString);

            return result;
        }

        private Task<T> ExecuteWithAuthenticatedClient<T>(Func<HttpClient, Task<T>> action, CancellationToken cancellationToken)
        {
            if (_token == null)
            {
                throw new ArgumentNullException("token", "An OAuth token is required with every request");
            }

            var activityId = this._activityManager.StartActivity(CancellationToken.None);

            var httpClient = new HttpClient(this._handler) { BaseAddress = this._baseAddr };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

            this._activityManager.StopActivity(activityId);

            return action(httpClient);
        }

        private Uri GetUriFromAction(string source, object values)
        {
            MethodInfo action = typeof(TApi).GetTypeInfo().GetDeclaredMethod(source);
            HttpMethodAttribute httpMethodInfo = action.GetCustomAttribute<HttpMethodAttribute>(true);
            var relativeAddr = httpMethodInfo.BuildRelativeUrl(values);
            return new Uri(relativeAddr);
        }
    }
}
