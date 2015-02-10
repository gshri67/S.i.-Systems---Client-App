using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class ConsultantServiceTests
    {
        private Mock<ISessionContext> _sessionContextMock;

        private const int CurrentUserClientId = 1;
        private const int AnotherClientId = 2;

        [SetUp]
        public void Setup()
        {
            _sessionContextMock = new Mock<ISessionContext>();
            _sessionContextMock.Setup(m => m.CurrentUser).Returns(() => new User
            {
                ClientId = CurrentUserClientId,
                FirstName = "Test",
                LastName = "User",
                Login = "Test.User",
                PasswordHash = "#"
            });
        }

        [Test]
        public void FindAlumni_WhenNoMutualContract_ShouldThrowUnauthorized()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>()
                });

            var service = new ConsultantService(repo.Object, _sessionContextMock.Object);
            Assert.Throws<UnauthorizedAccessException>(()=> service.Find(10));
        }

        [Test]
        public void FindAlumni_WhenMutualContract_ShouldNotThrow()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>
                    {
                        new Contract{ClientId = CurrentUserClientId}
                    }
                });

            var service = new ConsultantService(repo.Object, _sessionContextMock.Object);
            Assert.DoesNotThrow(() => service.Find(10));
        }

        [Test]
        public void FindAlumni_WhenMultipleContracts_ShouldFilterOutContractsWithOtherClients()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>
                    {
                        new Contract{ClientId = CurrentUserClientId},
                        new Contract{ClientId = AnotherClientId},
                        new Contract{ClientId = CurrentUserClientId}
                    }
                });

            var service = new ConsultantService(repo.Object, _sessionContextMock.Object);

            var consultant = service.Find(10);

            Assert.AreEqual(2, consultant.Contracts.Count());
            Assert.IsTrue(consultant.Contracts.All(c=>c.ClientId==CurrentUserClientId));
        }
    }
}
