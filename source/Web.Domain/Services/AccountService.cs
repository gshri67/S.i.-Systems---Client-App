using SiSystems.ClientApp.Web.Domain.Repositories;
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

        public AccountService(UserRepository userRepository)
        {

        }

        public async Task<bool> ForgotPassword(string emailAddress)
        {
            return false;
        }
    }
}
