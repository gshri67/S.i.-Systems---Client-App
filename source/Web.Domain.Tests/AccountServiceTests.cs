using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.MatchGuideService;
using SiSystems.ClientApp.Web.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        [Test]
        public async void ResetPassword_ShouldReturnTrue()
        {
            var clientMock = new Mock<IAccountService>();
            clientMock.Setup(mg => mg.ForgotPasswordAsync(It.IsAny<string>(), "Client")).ReturnsAsync(new LoginResponse());

            var sut = new AccountService(clientMock.Object);

            var response = await sut.ForgotPassword("bob.smith@email.com");
            Assert.IsTrue(response);

            clientMock.Verify(mg => mg.ForgotPasswordAsync("bob.smith@email.com", "Client"));
        }
    }
}
