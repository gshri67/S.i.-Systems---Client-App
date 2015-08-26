using Newtonsoft.Json;
using SiSystems.ClientApp.Web.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class AccountService
    {
        const string PortalName = "Client";
        private readonly HttpMessageHandler _httpHandler;
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository, HttpMessageHandler httpHandler)
        {
            this._userRepository = userRepository;
            this._httpHandler = httpHandler;
        }

        public async Task<ResetPasswordResult> ForgotPassword(string emailAddress)
        {
            if (!UserCanResetPassword(emailAddress))
            {
                return new ResetPasswordResult
                {
                    ResponseCode = -1,
                    Description =
                        "Your Client Portal account may not be activated. Please contact your Account Executive to resolve this issue."
                };
            }

            try
            {
                var response = await RequestResetPassword(emailAddress);
                return await ResetPasswordResult(response);
            }
            catch (HttpRequestException)
            {
                return new ResetPasswordResult
                {
                    ResponseCode = -1,
                    Description =
                        "There was a problem communicating with the system. Please use the online portal to reset your password."
                };
            }
        }

        private bool UserCanResetPassword(string emailAddress)
        {
            var clientContact = this._userRepository.FindByName(emailAddress);

            if (clientContact == null)
            {
                return false;
            }

            return true;
        }

        private static void EnsureAccountsServiceIsConfigured()
        {
            if (string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceUrl) ||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayId) ||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayPwd))
            {
                throw new NotImplementedException("No account service portal has been specified for this environment.");
            }
        }

        private static async Task<ResetPasswordResult> ResetPasswordResult(HttpResponseMessage response)
        {
            var json = await ParseResponse(response);

            if (!response.IsSuccessStatusCode)
            {
                return new ResetPasswordResult
                {
                    ResponseCode = -1,
                    Description = json
                };
            }

            var result = JsonConvert.DeserializeObject<ResetPasswordResult>(json);
            if (result.ResponseCode > 0)
            {
                result.Description = result.Description.Substring(0, result.Description.IndexOf('<'));
            }
            return result;
        }

        private static async Task<string> ParseResponse(HttpResponseMessage response)
        {
            return response != null && response.Content != null
                ? await response.Content.ReadAsStringAsync()
                : string.Empty;
        }

        private static async Task<HttpResponseMessage> RequestResetPassword(string emailAddress)
        {
            EnsureAccountsServiceIsConfigured();

            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideAccountServiceUrl) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("ForgotPassword/User?userEmail={0}&portal={1}", emailAddress, PortalName));
                request.Headers.Add("gatewayId", Settings.MatchGuideAccountServiceGatewayId);
                request.Headers.Add("gatewayPwd", Settings.MatchGuideAccountServiceGatewayPwd);

                var response = await httpClient.SendAsync(request);

                return response;
            }
        }
    }
}
