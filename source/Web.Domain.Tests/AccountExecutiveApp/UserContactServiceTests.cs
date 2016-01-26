
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Tests.AccountExecutiveApp
{
    [TestFixture]
    public class UserContactServiceTests
    {
        private Mock<IUserContactRepository> _userContactRepoMock;
        private Mock<ISessionContext> _session;

        [SetUp]
        public void Setup()
        {
            _userContactRepoMock = new Mock<IUserContactRepository>();

            _session = new Mock<ISessionContext>();
            _session.Setup(context => context.CurrentUser).Returns(new User
            {
                Id = 1
            });
        }

        [Test]
        public void UserContactLinkedInUrl_WithEmptyName_ReturnStrippedLinkedInUrl()
        {
            UserContact contractorContact = new UserContact();
            contractorContact.Id = 1;
            contractorContact.FirstName = "";
            contractorContact.LastName = "";
            _userContactRepoMock.Setup(repo => repo.GetUserContactById(1)).Returns(contractorContact);


            var service = new UserContactService(_userContactRepoMock.Object, _session.Object);
            var contractor = service.GetUserContactById(1);

            Assert.AreEqual(contractor.LinkedInUrl, "https://www.linkedin.com/vsearch/f?type=all&keywords=");
        }
        
        [Test]
        public void UserContactLinkedInUrl_WithMultipleSpaces_ReturnValidLinkedInUrlWithoutSpaces()
        {
            UserContact contractorContact = new UserContact();
            contractorContact.Id = 1;
            contractorContact.FirstName = " Bob Smith ";
            contractorContact.LastName = " Smitherson Smithy ";
            _userContactRepoMock.Setup(repo => repo.GetUserContactById(1)).Returns(contractorContact);

            var service = new UserContactService(_userContactRepoMock.Object, _session.Object);

            var contractor = service.GetUserContactById(1);

            Assert.AreEqual(contractor.LinkedInUrl, "https://www.linkedin.com/vsearch/f?type=all&keywords=Bob+Smith+++Smitherson+Smithy");
        }

        [Test]
        public void UserContactLinkedInUrl_WithNormalName_ReturnValidLinkedInUrl()
        {
            UserContact contractorContact = new UserContact();
            contractorContact.Id = 1;
            contractorContact.FirstName = "Bob";
            contractorContact.LastName = "Smith";
            _userContactRepoMock.Setup(repo => repo.GetUserContactById(1)).Returns(contractorContact);

            var service = new UserContactService(_userContactRepoMock.Object, _session.Object);

            var contractor = service.GetUserContactById(1);

            Assert.AreEqual(contractor.LinkedInUrl, "https://www.linkedin.com/vsearch/f?type=all&keywords=Bob+Smith");
        }
    }
}


