using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services;
using ClientApp.Services.Interfaces;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Tests.Services
{
    [TestFixture]
    public class LogoutServiceTests
    {
        [Test]
        public void Logout_ClearsConnectionToken()
        {
            var connection = new Mock<IConnectionService>();
            connection.Object.Token = new OAuthToken();
            var logoutService = new LogoutService(connection.Object);

            logoutService.Logout();
            connection.VerifySet(c => c.Token = null);
        }
    }
}
