using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    [Category("Database")]
    public class UserRepositoryTests
    {
        private UserRepository _repo;

        [SetUp]
        public void SetUp()
        {
            this._repo = new UserRepository();
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
