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
            var clientContact = this._userRepository.FindByName(emailAddress);
            if (clientContact == null)
            {
                return new ResetPasswordResult { ResponseCode = -1, Description = "Please contact your AE." };
            }
            if (!(clientContact.ClientPortalType == MatchGuideConstants.ClientPortalType.PortalContact 
                || clientContact.ClientPortalType == MatchGuideConstants.ClientPortalType.PortalAdministrator))
            {
                return new ResetPasswordResult { ResponseCode = -1, Description = "Your account does not have access. Please contact your AE." };
            }

            if (string.IsNullOrEmpty(Settings.MatchGuideAccountServiceUrl))
            {
                throw new NotImplementedException("No account service URL has been specified for this environment.");
            }
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideAccountServiceUrl) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("ForgotPassword/User?userEmail={0}&portal={1}", emailAddress, PortalName));
                request.Headers.Add("gatewayId", "rxH4mJUatb6xYYpXsJsaAw==");
                request.Headers.Add("gatewayPwd", "D1MajGi5bVtuY5GVSx6nyQ==");

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResetPasswordResult>(json);
                }
            }
            return new ResetPasswordResult { ResponseCode = -1, Description = "Your password could not be reset. Please contact your AE for more information." };
        }
    }
}
