using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    [Category("Database")]
    public class CompanyRepositoryTests
    {
        private CompanyRepository _repo;

        private int _companyWithDivisionId = 1;
        private int _companyWithNoDivisionId = 2;
        private int _divisionId = 4;

        [SetUp]
        public void Setup()
        {
            _repo = new CompanyRepository(new MockCache());
        }

        [Test]
        public void GetAllAssociatedCompanyIds_ShouldIncludeDivision()
        {
            var ids = _repo.GetAllAssociatedCompanyIds(_companyWithDivisionId).ToList();

            Assert.Contains(_divisionId, ids);
        }

        [Test]
        public void GetAllAssociatedCompanyIds_HasDivision_ShouldIncludeOwnId()
        {
            var ids = _repo.GetAllAssociatedCompanyIds(_companyWithDivisionId).ToList();

            Assert.Contains(_companyWithDivisionId, ids);
        }

        [Test]
        public void GetAllAssociatedCompanyIds_NoDivision_ShouldIncludeOwnIdOnly()
        {
            var ids = _repo.GetAllAssociatedCompanyIds(_companyWithNoDivisionId).ToList();

            Assert.AreEqual(1, ids.Count);
            Assert.Contains(_companyWithNoDivisionId, ids);
        }
    }

    class MockCache : IObjectCache
    {
        private readonly Dictionary<string, object> _mockCache = new Dictionary<string, object>(); 

        public void AddItem(string key, object value)
        {
            _mockCache.Add(key, value);
        }

        public object GetItem(string key)
        {
            return _mockCache.ContainsKey(key) ? _mockCache[key] : null;
        }
    }
}
