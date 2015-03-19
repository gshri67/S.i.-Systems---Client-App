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
using Xamarin;

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
            try
            {
                var url = GetRelativeUriFromAction(caller, null);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "username", WebUtility.HtmlEncode (username) },
                        { "password", WebUtility.HtmlEncode (password) },
                        { "grant_type", "password" }
                    })
                };

                var response = await this.ExecuteWithDefaultClient(request, false);

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

                    Insights.Identify(token.Username, new Dictionary<string, string>
                    {
                        { "Token Expires At", token.ExpiresAt },
                        { "Token Expires In", token.ExpiresIn.ToString() },
                        { "Token Issued At", token.IssuedAt }
                    });
                    Insights.Track(TrackId.LoginSuccess, Insights.Traits.Email, username);
                    return new ValidationResult { IsValid = true };
                }

                var error = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                Insights.Track(TrackId.LoginFailure, new Dictionary<string, string>
                {
                    {Insights.Traits.Email, username },
                    {"Error", error.Error },
                    {"Error Description", error.ErrorDescription }
                });

                return new ValidationResult { IsValid = false, Message = error.ErrorDescription };
            }
            catch (Exception e)
            {
                Insights.Report(e, new Dictionary<string, string>
                {
                    { Insights.Traits.Email, username }
                });
                return new ValidationResult { IsValid = false, Message = "Error executing login request. " + e.Message };
            }
        }

        public async Task Deauthenticate([CallerMemberName] string caller = null)
        {
            this._tokenStore.DeleteDeviceToken();
            await this.Post(null, caller);
            this._token = null;
        }

        public Task Post(object content, [CallerMemberName] string caller = null)
        {
            var url = GetRelativeUriFromAction(caller, null);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (content != null)
            {
                request.Content = content as HttpContent ?? 
                    new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");
            }
            return this.ExecuteWithDefaultClient(request);
        }

        public Task<TResult> PostUnauthenticated<TResult>(object content, [CallerMemberName] string caller = null)
        {
            return this.Post<TResult>(content, caller, false);
        }

        private Task<TResult> Post<TResult>(object content, string caller, bool authenticate)
        {
            var url = GetRelativeUriFromAction(caller, null);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (content != null)
            {
                request.Content = content as HttpContent ??
                    new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");
            }
            return this.Execute<TResult>(request, authenticate);
        }

        public Task<TResult> Get<TResult>(object parameters = null, [CallerMemberName] string caller = null)
        {
            var url = GetRelativeUriFromAction(caller, parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return this.Execute<TResult>(request);
        }

        public Task<TResult> GetUnauthenticated<TResult>(object parameters = null, [CallerMemberName] string caller = null)
        {
            var url = GetRelativeUriFromAction(caller, parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return this.Execute<TResult>(request, false);
        }

        private async Task<TResult> Execute<TResult>(HttpRequestMessage request, bool authenticate = true)
        {
            var response = await this.ExecuteWithDefaultClient(request, authenticate);
            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonString);
            }
            return default(TResult);
        }

        private async Task<HttpResponseMessage> ExecuteWithDefaultClient(HttpRequestMessage request, bool authenticate = true)
        {
            try
            {
                this._activityManager.StartActivity();

                var httpClient = new HttpClient(this._handler) { BaseAddress = this.BaseAddress };

                if (authenticate)
                {
                    if (this._token == null)
                    {
                        this._errorSource.ReportError("TokenExpired", null);
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);
                }

                var response = await httpClient.SendAsync(request);

                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

                if ((int)response.StatusCode >= 300)
                {
                    Insights.Track("UnexpectedStatusCode", new Dictionary<string, string>
                    {
                        {"Status Code", response.StatusCode.ToString()},
                        {"Request URL", request.RequestUri.ToString()},
                        {"Request Method", request.Method.Method}
                    });
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        this._tokenStore.DeleteDeviceToken();
                        var error = JsonConvert.DeserializeObject<ApiErrorResponse>(content);
                        this._errorSource.ReportError("TokenExpired", error.ErrorDescription, true);
                        return response;
                    case HttpStatusCode.InternalServerError:
                        var serverError = JsonConvert.DeserializeObject<HttpError>(content);
                        this._errorSource.ReportError(null, serverError.Message);
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
                this._activityManager.StopActivity();
            }
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
