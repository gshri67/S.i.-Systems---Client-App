using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class ConsultantMessageServiceTests
    {

        [Test]
        public void SendMessage_WhenUserCompanyDoesNotShareContract_ShouldThrowUnauthorized()
        {
            var consultantRepoMock = new Mock<IConsultantRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();
            var sessionContextMock = new Mock<ISessionContext>();

            consultantRepoMock.Setup(m => m.Find(It.IsAny<int>()))
                .Returns(new Consultant
                {
                    Id = 99,
                    Contracts = new List<Contract> {new Contract{ClientId = 12}}
                });

            sessionContextMock.Setup(s => s.CurrentUser)
                .Returns(new User
                {
                    ClientId = 1
                });

            companyRepoMock.Setup(m => m.GetAllAssociatedCompanyIds(It.IsAny<int>()))
                .Returns(new List<int> {1, 2, 3, 4});

            var service = new ConsultantMessageService(consultantRepoMock.Object, companyRepoMock.Object,
                sessionContextMock.Object);

            Assert.Throws<UnauthorizedAccessException>(() => service.SendMessage(new ConsultantMessage
            {
                ConsultantId = 99,
                Text = "Hey there!"
            }));

        }
    }
}
