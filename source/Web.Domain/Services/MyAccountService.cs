using Newtonsoft.Json;
using SiSystems.ClientApp.Web.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class MyAccountService
    {
        const string PortalName = "Client";
        private readonly HttpMessageHandler _httpHandler;
        private readonly IUserRepository _userRepository;
        private ISessionContext _session;

        public MyAccountService(IUserRepository userRepository, ISessionContext session, HttpMessageHandler httpHandler)
        {
            this._userRepository = userRepository;
            this._httpHandler = httpHandler;
            _session = session;
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

        public async Task<IEnumerable<int>> RequestERemittancePDF
            (Remittance remittance)
        {
            EnsureMyAccountsServiceIsConfigured();

            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideMyAccountServiceUrl) })
            {
                string candidateId = _session.CurrentUser.Id.ToString();
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format("MyaccountService.svc/ERemittancePDF/{0}/GetPDF?UV1={1}&UV2={2}&UV3={3}&UV4={4}", candidateId, remittance.VoucherNumber, remittance.Source, remittance.DepositDate.ToString("MM/dd/yyyy"), remittance.DBSource));

                HttpResponseMessage response = null;

                if( httpClient != null && request != null )
                   response = await httpClient.SendAsync(request);

                if (response == null)
                {
                    throw new Exception("Response is Null");
                }

                Stream stream = await response.Content.ReadAsStreamAsync();
                byte[] buffer = new byte[(int)stream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.Length);

                int numBytesInInt = sizeof (Int32);
                int intArraySize = (int) stream.Length/numBytesInInt;
                
                if ((int) stream.Length%numBytesInInt > 0)
                {
                    intArraySize ++;
                }

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);

                int[] intBuffer = new int[intArraySize];
                buffer.CopyTo(intBuffer, 0);
                
                for( int i = 0; i < intArraySize; i ++ )
                    intBuffer[i] = BitConverter.ToInt32(buffer, 4*i);

                return intBuffer.AsEnumerable();
            }
        }
    }
}
