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
        private readonly ISessionContext _session;

        public MyAccountService(ISessionContext session)
        {
            _session = session;
        }

        private static void EnsureMyAccountsServiceIsConfigured()
        {
            if (string.IsNullOrWhiteSpace(Settings.MatchGuideMyAccountServiceUrl))
            {
                throw new NotImplementedException("No account service portal has been specified for this environment.");
            }
        }

        public async Task<HttpResponseMessage> RequestERemittancePDF(Remittance remittance)
        {
            EnsureMyAccountsServiceIsConfigured();

            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Settings.MatchGuideMyAccountServiceUrl) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, 
                    string.Format("ERemittancePDF/{0}/GetPDF?UV1={1}&UV2={2}&UV3={3}&UV4={4}", 
                                    _session.CurrentUser.Id, 
                                    remittance.VoucherNumber, 
                                    remittance.Source, 
                                    remittance.DepositDate.ToString("MM/dd/yyyy"), 
                                    remittance.DBSource)
                    );

                var response = await httpClient.SendAsync(request);

                if (response == null)
                {
                    throw new Exception("Unable to retrieve the PDF at this time.");
                }

                return response;
            }
        }
    }
}
