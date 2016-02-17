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
using SiSystems.ClientApp.Web.Domain;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class MyAccountService
    {
        const string PortalName = "Client";
        private readonly HttpMessageHandler _httpHandler;
        private readonly IUserRepository _userRepository;

        public MyAccountService(IUserRepository userRepository, HttpMessageHandler httpHandler)
        {
            this._userRepository = userRepository;
            this._httpHandler = httpHandler;
        }

        private static void EnsureMyAccountsServiceIsConfigured()
        {
            if (string.IsNullOrWhiteSpace(Settings.MatchGuideMyAccountServiceUrl) )
            /*||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayId) ||
                string.IsNullOrWhiteSpace(Settings.MatchGuideAccountServiceGatewayPwd))*/
            {
                throw new NotImplementedException("No account service portal has been specified for this environment.");
            }
        }

        private static async Task<string> ParseResponse(HttpResponseMessage response)
        {
            return response != null && response.Content != null
                ? await response.Content.ReadAsStringAsync()
                : string.Empty;
        }
        /*
        public async Task<ResetPasswordResult> RequestPDF(string candidateId, string VcherNumber, string Source, string DocDate, string DBSource)
        {
            try
            {
                var response = await RequestERemittancePDF(candidateId, VcherNumber, Source, DocDate, DBSource);
                return response;
            }
            catch (HttpRequestException)
            {
            }
        }
        */
        public async Task<HttpResponseMessage> RequestERemittancePDF
            (string candidateId, string VcherNumber, string Source, string DocDate, string DBSource)
        {
            EnsureMyAccountsServiceIsConfigured();

            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideMyAccountServiceUrl) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/ERemittancePDF/{0}/GetPDF?UV1={1}&UV2={2}&UV3={3}&UV4={4}", candidateId, VcherNumber, Source, DocDate, DBSource));
                //request.Headers.Add("gatewayId", Settings.MatchGuideMyAccountServiceGatewayId);
                //request.Headers.Add("gatewayPwd", Settings.MatchGuideMyAccountServiceGatewayPwd);

                var response = await httpClient.SendAsync(request);

                return response;
            }
        }
    }
}
