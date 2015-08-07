using System;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    [Category("Database")]
    public class ClientDetailsRepositoryTests
    {
        private ClientDetailsRepository _repo;

        private Mock<ICompanyRepository> _companyRepository;

        private DateTime ExpiryDate = new DateTime(2015, 3, 20);

        private const int InvalidCompanyId = 0;

        [SetUp]
        public void Setup()
        {
            _companyRepository = new Mock<ICompanyRepository>();
            _repo = new ClientDetailsRepository(_companyRepository.Object);
        }

        [Test]
        public void GetClientDetails_WhenInvalidId_ShouldGetEmptyDetails()
        {
            var emptyClientDetails = new ClientAccountDetails();
            var clientDetailsFromRepo = _repo.GetClientDetails(InvalidCompanyId);


            Assert.AreEqual(emptyClientDetails.FloThruFee, clientDetailsFromRepo.FloThruFee);
            Assert.AreEqual(emptyClientDetails.FloThruFeePayment, clientDetailsFromRepo.FloThruFeePayment);
            Assert.AreEqual(emptyClientDetails.FloThruFeeType, clientDetailsFromRepo.FloThruFeeType);
            Assert.AreEqual(emptyClientDetails.InvoiceFormat, clientDetailsFromRepo.InvoiceFormat);
            Assert.AreEqual(emptyClientDetails.ClientInvoiceFrequency, clientDetailsFromRepo.ClientInvoiceFrequency);
            Assert.AreEqual(emptyClientDetails.MspFeePercentage, clientDetailsFromRepo.MspFeePercentage);
            Assert.AreEqual(emptyClientDetails.MaxVisibleRate, clientDetailsFromRepo.MaxVisibleRate);
        }
    }
}
