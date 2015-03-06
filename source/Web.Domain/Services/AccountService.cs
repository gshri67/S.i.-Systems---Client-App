using SiSystems.ClientApp.Web.Domain.MatchGuideService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class AccountService
    {
        const string PortalName = "Client";

        private readonly IAccountService _client;

        public AccountService(IAccountService client)
        {
            this._client = client;
        }

        public async Task<bool> ForgotPassword(string emailAddress)
        {
            var response = await this._client.ForgotPasswordAsync(emailAddress, PortalName);
            return true;
        }
    }
}
