using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            if (string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceUrl) ||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayId) ||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayPwd))
            {
                throw new NotImplementedException("No account service portal has been specified for this environment.");
            }

            var errorResponse = new ResetPasswordResult
            {
                ResponseCode = -1,
                Description = "Your Client Portal account may not be activated. Please contact your Account Executive to resolve this issue."
            };

            var clientContact = this._userRepository.FindByName(emailAddress);
            if (clientContact == null)
            {
                return errorResponse;
            }
            if (!(clientContact.ClientPortalType == MatchGuideConstants.ClientPortalType.PortalContact 
                || clientContact.ClientPortalType == MatchGuideConstants.ClientPortalType.PortalAdministrator))
            {
                return errorResponse;
            }

            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideAccountServiceUrl) })
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("ForgotPassword/User?userEmail={0}&portal={1}", emailAddress, PortalName));
                    request.Headers.Add("gatewayId", Settings.MatchGuideAccountServiceGatewayId);
                    request.Headers.Add("gatewayPwd", Settings.MatchGuideAccountServiceGatewayPwd);

                    var response = await httpClient.SendAsync(request);

                    var json = response != null && response.Content != null
                        ? await response.Content.ReadAsStringAsync()
                        : string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ResetPasswordResult>(json);
                        if (result.ResponseCode > 0)
                        {
                            result.Description = result.Description.Substring(0, result.Description.IndexOf('<'));
                        }
                        return result;
                    }
                    errorResponse.Description = json;
                }
                catch (Exception e)
                {
                    errorResponse.Description = "An error has occurred while attempting to contact the server. Please try again.";
                }
                
            }

            return errorResponse;
        }
    }
}
