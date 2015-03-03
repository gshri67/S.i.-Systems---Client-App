﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class ClientDetailsRepositoryTests
    {
        private ClientDetailsRepository _repo;

        private const int InvalidCompanyId = 0;
        private const int ValidCompanyIdWithEmptyDetails = 1;

        [SetUp]
        public void Setup()
        {
            _repo = new ClientDetailsRepository();
        }

        [Test]
        public void GetClientDetails_WhenInvalidId_ShouldGetEmptyDetails()
        {
            var emptyClientDetails = new ClientAccountDetails();
            var clientDetailsFromRepo = _repo.GetClientDetails(InvalidCompanyId);


            Assert.AreEqual(emptyClientDetails.FloThruFee, clientDetailsFromRepo.FloThruFee);
            Assert.AreEqual(emptyClientDetails.FloThruFeePayer, clientDetailsFromRepo.FloThruFeePayer);
            Assert.AreEqual(emptyClientDetails.FloThruFeeType, clientDetailsFromRepo.FloThruFeeType);
            Assert.AreEqual(emptyClientDetails.InvoiceFormat, clientDetailsFromRepo.InvoiceFormat);
            Assert.AreEqual(emptyClientDetails.InvoiceFrequency, clientDetailsFromRepo.InvoiceFrequency);
        }

        [Test]
        public void GetClientDetails_WhenValidIdForEmptyResults_ShouldGetEmptyDetails()
        {
            var emptyClientDetails = new ClientAccountDetails();
            var clientDetailsFromRepo = _repo.GetClientDetails(ValidCompanyIdWithEmptyDetails);


            Assert.AreEqual(emptyClientDetails.FloThruFee, clientDetailsFromRepo.FloThruFee);
            Assert.AreEqual(emptyClientDetails.FloThruFeePayer, clientDetailsFromRepo.FloThruFeePayer);
            Assert.AreEqual(emptyClientDetails.FloThruFeeType, clientDetailsFromRepo.FloThruFeeType);
            Assert.AreEqual(emptyClientDetails.InvoiceFormat, clientDetailsFromRepo.InvoiceFormat);
            Assert.AreEqual(emptyClientDetails.InvoiceFrequency, clientDetailsFromRepo.InvoiceFrequency);
        }
    }
}
