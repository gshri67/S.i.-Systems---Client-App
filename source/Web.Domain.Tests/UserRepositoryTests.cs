using NUnit.Framework;
using Moq;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;
using System;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    [Category("Database")]
    public class UserRepositoryTests
    {
        private UserRepository _repo;

        private Mock<IClientDetailsRepository> _detailsRepo;

        private DateTime ExpiryDate = new DateTime(2015, 3, 20);

        [SetUp]
        public void SetUp()
        {
            this._detailsRepo = new Mock<IClientDetailsRepository>();
            this._repo = new UserRepository(_detailsRepo.Object);
        }

        [Test]
        public void ShouldNotBeUsingConfiguredCompaniesOnceDatabaseIsReady()
        {
            Assert.IsTrue(DateTime.Today < ExpiryDate, "User repository is still using the company list from configuration. It should be removed by now.");
        }

        [Test]
        public void FindById_ClientContact_ShouldHavePortalContactAccess()
        {
            var contact = _repo.Find(1);

            Assert.AreEqual(MatchGuideConstants.UserType.ClientContact, contact.UserType);
            Assert.AreEqual(MatchGuideConstants.ClientPortalType.PortalContact, contact.ClientPortalType);
        }
    }
}
