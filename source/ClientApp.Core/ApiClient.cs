using ClientApp.Core.HttpAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class ApiClient<TApi> : IApiClient
    {
        private readonly ITokenStore _tokenStore;

        private readonly IActivityManager _activityManager;

        private readonly IErrorSource _errorSource;

        private readonly HttpMessageHandler _handler;

        private OAuthToken _token;

        public Uri BaseAddress { get; set; }

        public ApiClient(ITokenStore tokenStore, IActivityManager activityManager, IErrorSource errorSource, IHttpMessageHandlerFactory handlerFactory)
        {
            this._tokenStore = tokenStore;
            this._handler = handlerFactory.Get();
            this._activityManager = activityManager;
            this._errorSource = errorSource;
            this._token = this._tokenStore.GetDeviceToken();

            this.BaseAddress = new Uri(typeof(TApi).GetTypeInfo().GetCustomAttribute<ApiAttribute>().BaseUrl);
        }

        public async Task<ValidationResult> Authenticate(string username, string password, [CallerMemberName] string caller = null)
        {
            var response = await this.ExecuteWithDefaultClient(async httpClient =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, GetRelativeUriFromAction(caller, null))
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "username", WebUtility.HtmlEncode (username) },
                        { "password", WebUtility.HtmlEncode (password) },
                        { "grant_type", "password" }
                    })
                };

                return await httpClient.SendAsync(request);
            });
            try
            {
                string json = null;
                if (response.Content != null)
                {
                    json = await response.Content.ReadAsStringAsync();
                }
                if (response.IsSuccessStatusCode)
                {
                    var token = JsonConvert.DeserializeObject<OAuthToken>(json);
                    this._token = _tokenStore.SaveToken(token);
                    _tokenStore.SaveUserName(token.Username);

                    return new ValidationResult { IsValid = true };
                }

                var error = JsonConvert.DeserializeObject<ApiErrorResponse>(json);
                return new ValidationResult { IsValid = false, Message = error.ErrorDescription };
            }
            catch (Exception e)
            {
                return new ValidationResult { IsValid = false, Message = "Error executing login request. " + e.Message };
            }
        }

        public async Task Deauthenticate([CallerMemberName] string caller = null)
        {
            await this.ExecuteWithAuthenticatedClient(async httpClient => await httpClient.PostAsync(GetRelativeUriFromAction(caller, null), null));
            this._tokenStore.DeleteDeviceToken();
        }

        public Task Post(HttpContent content, [CallerMemberName] string caller = null)
        {
            return this.ExecuteWithAuthenticatedClient(async httpClient => await httpClient.PostAsync(GetRelativeUriFromAction(caller, null), content));
        }

        public async Task<TResult> PostUnauthenticated<TResult>(HttpContent content, [CallerMemberName] string caller = null)
        {
            var response = await this.ExecuteWithDefaultClient(async httpClient => await httpClient.PostAsync(GetRelativeUriFromAction(caller, null), content));

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonString);
            }
            return default(TResult);
        }

        public async Task<TResult> Get<TResult>([CallerMemberName] string caller = null)
        {
            return await Get<TResult>(null, caller);
        }

        public async Task<TResult> Get<TResult>(object parameters, [CallerMemberName] string caller = null)
        {
            var response = await ExecuteWithAuthenticatedClient(async httpClient => await httpClient.GetAsync(GetRelativeUriFromAction(caller, parameters), HttpCompletionOption.ResponseHeadersRead));

            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonString);
            }
            return default(TResult);
        }

        private async Task<HttpResponseMessage> ExecuteWithDefaultClient(Func<HttpClient, Task<HttpResponseMessage>> action)
        {
            var activityId = this._activityManager.StartActivity(CancellationToken.None);

            try
            {
                var httpClient = new HttpClient(this._handler) { BaseAddress = this.BaseAddress };

                var response = await action(httpClient);

                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        this._tokenStore.DeleteDeviceToken();
                        var error = JsonConvert.DeserializeObject<ApiErrorResponse>(content);
                        this._errorSource.ReportError("TokenExpired", error.ErrorDescription, true);
                        return response;
                    default:
                        return response;
                }
            }
            catch (Exception exception)
            {
                this._errorSource.ReportError(null, exception.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(exception.Message) };
            }
            finally
            {
                this._activityManager.StopActivity(activityId);
            }
        }

        private async Task<HttpResponseMessage> ExecuteWithAuthenticatedClient(Func<HttpClient, Task<HttpResponseMessage>> action)
        {
            return await this.ExecuteWithDefaultClient(async httpClient =>
            {
                if (this._token == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("{\"error\":\"no_token\",\"error_description\":\"Token has expired.\"}")
                    };
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);
                return await action(httpClient);
            });
        }

        private Uri GetRelativeUriFromAction(string source, object values)
        {
            MethodInfo action = typeof(TApi).GetTypeInfo().GetDeclaredMethod(source);
            HttpMethodAttribute httpMethodInfo = action.GetCustomAttribute<HttpMethodAttribute>(true);
            var relativeAddr = httpMethodInfo.BuildRelativeUrl(values);
            return new Uri(relativeAddr, UriKind.Relative);
        }
    }
}
