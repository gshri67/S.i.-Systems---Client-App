using NUnit.Framework;
using Moq;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class MatchGuideServiceIntegrationTests
    {
        [Test]
        [Category("Integration")]
        public async void ForgotPassword_AllAccessClient_ShouldSucceed()
        {
            const string username = "mrestall@suncor.com";
            var userObject = new User {
                Id = 130577,
                Login = "mrestall@suncor.com",
                FirstName = "Martin", LastName = "Restall",
                ClientId = 50256,
                CompanyName = "Suncor Energy Inc.",
                ClientPortalType = 833,
                UserType = 491 };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.FindByName(username)).Returns(userObject);

            var service = new AccountService(userRepositoryMock.Object);

            var result = await service.ForgotPassword(username);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess, result.Description);
        }

        [Test]
        [Category("Integration")]
        public async void ForgotPassword_PortalAdministratorClient_ShouldSucceed()
        {
            const string username = "mrestall@suncor.com";
            var userObject = new User
            {
                Id = 115624,
                Login = "Husky Energy Inc.",
                FirstName = "Brenda",
                LastName = "Buchanan",
                ClientId = 49167,
                CompanyName = "Husky Energy Inc.",
                ClientPortalType = 834,
                UserType = 491
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.FindByName(username)).Returns(userObject);

            var service = new AccountService(userRepositoryMock.Object);

            var result = await service.ForgotPassword(username);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess, result.Description);
        }

        [Test]
        [Category("Integration")]
        public async void ForgotPassword_NoAccessClient_ShouldFail()
        {
            const string username = "mrestall@suncor.com";
            var userObject = new User
            {
                Id = 120903,
                Login = "vamay@suncor.com",
                FirstName = "Valerie",
                LastName = "May",
                ClientId = 50256,
                CompanyName = "Suncor Energy Inc.",
                ClientPortalType = 835,
                UserType = 491
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.FindByName(username)).Returns(userObject);

            var service = new AccountService(userRepositoryMock.Object);

            var result = await service.ForgotPassword(username);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess, result.Description);
            Assert.AreEqual(result.Description, "Your account does not have access. Please contact your AE.");
        }
    }
}
