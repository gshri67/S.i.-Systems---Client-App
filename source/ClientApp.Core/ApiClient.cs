﻿using ClientApp.Core.HttpAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class ApiClient<TApi>
    {
        private readonly ITokenStore _tokenStore;

        private readonly IActivityManager _activityManager;

        private readonly HttpMessageHandler _handler;

        private OAuthToken _token;

        public Uri BaseAddress { get; set; }

        public ApiClient(ITokenStore tokenStore, IActivityManager activityManager, HttpMessageHandler handler)
        {
            this._tokenStore = tokenStore;
            this._handler = handler;
            this._activityManager = activityManager;
            this._token = this._tokenStore.Fetch();
            this.BaseAddress = new Uri(typeof(TApi).GetTypeInfo().GetCustomAttribute<ApiAttribute>().BaseUrl);
        }

        public async Task<ValidationResult> Authenticate(string username, string password, [CallerMemberName] string caller = null)
        {
            var activityId = this._activityManager.StartActivity(CancellationToken.None);

            try
            {
                var httpClient = new HttpClient(this._handler) { BaseAddress = BaseAddress };
                var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "username", WebUtility.HtmlEncode (username) },
                        { "password", WebUtility.HtmlEncode (password) },
                        { "grant_type", "password" }
                    });

                var request = new HttpRequestMessage(HttpMethod.Post, GetRelativeUriFromAction(caller, null))
                {
                    Content = content
                };

                var authenticationResponse = await httpClient.SendAsync(request);

                var jsonString = await authenticationResponse.Content.ReadAsStringAsync();

                if (authenticationResponse.IsSuccessStatusCode)
                {
                    var token = JsonConvert.DeserializeObject<OAuthToken>(jsonString);
                    this._token = _tokenStore.Store(token);

                    return new ValidationResult { IsValid = true };
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(jsonString);
                    return new ValidationResult { IsValid = false, Message = error.ErrorDescription };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult { IsValid = false, Message = ex.Message };
            }
            finally
            {
                this._activityManager.StopActivity(activityId);
            }
        }

        public async Task<TResult> Get<TResult>(object parameters, [CallerMemberName] string caller = null)
        {
            return await Get<TResult>(parameters, CancellationToken.None, caller);
        }

        public async Task<TResult> Get<TResult>(object values, CancellationToken cancellationToken, [CallerMemberName] string caller = null)
        {
            var response = await ExecuteWithAuthenticatedClient(async httpClient => await httpClient.GetAsync(GetRelativeUriFromAction(caller, values), HttpCompletionOption.ResponseHeadersRead, cancellationToken), cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonString);
            }
            return default(TResult);
        }

        private Task<T> ExecuteWithAuthenticatedClient<T>(Func<HttpClient, Task<T>> action, CancellationToken cancellationToken)
        {
            if (_token == null)
            {
                throw new ArgumentNullException("token", "An OAuth token is required with every request");
            }

            var activityId = this._activityManager.StartActivity(CancellationToken.None);

            var httpClient = new HttpClient(this._handler) { BaseAddress = this.BaseAddress };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

            this._activityManager.StopActivity(activityId);

            return action(httpClient);
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
