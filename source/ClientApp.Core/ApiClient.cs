using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Xamarin;

using ClientApp.Core.HttpAttributes;
using ClientApp.Core.Platform;

namespace ClientApp.Core
{
    public abstract class ApiClient
    {
        private readonly HttpMessageHandler _handler;

        private readonly IActivityManager _activityManager;

        private readonly ITokenStore _tokenStore;

        private readonly IErrorSource _errorSource;

        public readonly Uri BaseAddress;
        public readonly Uri DemoBaseAddress;
        public string Username;

        public TimeSpan Timeout = TimeSpan.FromSeconds(100);

        public ApiClient(ITokenStore tokenStore, IActivityManager activityManager, IErrorSource errorSource, IHttpMessageHandlerFactory handlerFactory)
        {
            this._tokenStore = tokenStore;
            this._handler = handlerFactory.Get();
            this._activityManager = activityManager;
            this._errorSource = errorSource;

            this.BaseAddress = new Uri(this.GetType().GetTypeInfo().GetCustomAttribute<ApiAttribute>().BaseUrl);
            this.DemoBaseAddress = new Uri(Settings.DemoMatchGuideApiAddress);
        }

        /// <summary>
        /// Execute a request and deserialize the result into the given <see cref="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The expected type returned in the response.</typeparam>
        /// <param name="data">The data to send with the request. For a post, the data will be serialized to JSON unless it is passed in as HttpContent.</param>
        /// <param name="caller">The source of the request.</param>
        /// <returns>An object of type <see cref="TResult"/></returns>
        protected async Task<TResult> ExecuteWithDefaultClient<TResult>(object data = null, [CallerMemberName] string caller = null)
        {
            var response = await this.ExecuteWithDefaultClient(data, caller);
            if (response != null && response.IsSuccessStatusCode && response.Content != null)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonString);
            }
            return default(TResult);
        }

        private static bool IsDemoUser(string username)
        {
            return username != null 
                && username.ToLower().Contains(Settings.DemoDomain.ToLower());
        }

        private Uri BaseAddressForUsername(string username)
        {
            return IsDemoUser(username)
                ? this.DemoBaseAddress
                : this.BaseAddress;
        }

        /// <summary>
        /// Execute a request using an http client with the authorization header pre-set (using the user's token if available). 
        /// On a forbidden or unauthorized response, will broadcast a "TokenExpired" message to the client.
        /// </summary>
        /// <param name="data">The data to send with the request. For a post, the data will be serialized to JSON unless it is passed in as HttpContent.</param>
        /// <param name="caller">The source of the request.</param>
        /// <returns>An http response message.</returns>
        protected async Task<HttpResponseMessage> ExecuteWithDefaultClient(object data = null, [CallerMemberName] string caller = null, HttpMethod method = null)
        {
            try
            {
                this._activityManager.StartActivity();

                var httpClient = new HttpClient(this._handler)
                {
                    BaseAddress = BaseAddressForUsername(this.Username),
                    Timeout = this.Timeout
                };

                var token = this._tokenStore.GetDeviceToken();
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    httpClient.BaseAddress = BaseAddressForUsername(token.Username);
                }

                var request = BuildRequest(caller, data);
                if (request.Method == HttpMethod.Post && data != null)
                {
                    request.Content = data as HttpContent ??
                        new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                }

                var response = await httpClient.SendAsync(request);
                var responseContent = response.Content != null 
                    ? await response.Content.ReadAsStringAsync() 
                    : null;

                if (!response.IsSuccessStatusCode)
                {
                    Insights.Track("Request Failed", new Dictionary<string, string>
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
                        var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent);
                        this._errorSource.ReportError("TokenExpired", error.ErrorDescription, true);
                        return response;
                    case HttpStatusCode.InternalServerError:
                        var serverError = JsonConvert.DeserializeObject<HttpError>(responseContent);
                        this._errorSource.ReportError(null, serverError.Message);
                        return response;
                    default:
                        return response;
                }
            }
            catch (OperationCanceledException exception)
            {
                this._errorSource.ReportError("Timeout", "The request timed out.", true);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(exception.Message) };
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

        private HttpRequestMessage BuildRequest(string source, object values, HttpMethod method = null)
        {
            var request = new HttpRequestMessage();

            MethodInfo action = this.GetType().GetTypeInfo().GetDeclaredMethod(source);
            if (action == null && method == null)
            {
                throw new ArgumentException("The request must be configured via a HttpMethodAttribute or specified explicitly.");
            }

            var httpMethodInfo = action != null
                ? action.GetCustomAttribute<HttpMethodAttribute>(true)
                : method == HttpMethod.Post
                    ? (HttpMethodAttribute)(new HttpPostAttribute(source))
                    : (HttpMethodAttribute)(new HttpGetAttribute(source));

            var relativeAddr = httpMethodInfo.BuildRelativeUrl(values);
            var url = new Uri(relativeAddr, UriKind.Relative);

            return new HttpRequestMessage(httpMethodInfo.Type, url);
        }
    }
}
