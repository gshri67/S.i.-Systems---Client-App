using System;
using System.Collections.Generic;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Tests.ViewModels
{
    [TestFixture]
    public class NewContractViewModelTests
    {
        private NewContractViewModel _vm;

        [SetUp]
        public void SetUp()
        {
            _vm = new NewContractViewModel
                  {
                      StartDate = DateTime.Now,
                      EndDate = DateTime.Now.AddDays(1),
                      ApproverEmail = "test@test.com",
                      ContractorRate = 100,
                      ContractTitle = "Senior Developer",
                      Specialization = new Specialization { Id = 4, Name = "Project Management"}
                  };
        }

        [Test]
        public void Consultant_SettingAlsoSetsRate()
        {
            var consultant = new Consultant();
            consultant.Contracts.Add(new Contract{EndDate = DateTime.MaxValue, Rate = 80});

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.ContractorRate);
        }

        [Test]
        public void Consultant_RateSetUsingLastContract()
        {
            var consultant = new Consultant();
            consultant.Contracts.Add(new Contract { EndDate = DateTime.MaxValue, Rate = 80 });
            consultant.Contracts.Add(new Contract { EndDate = DateTime.MinValue, Rate = 120 });

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.ContractorRate);
        }

        [Test]
        public void TotalRate_AddsContractorAndServiceRate()
        {
            _vm.ContractorRate = 80;
            Assert.AreEqual(80 + NewContractViewModel.ServiceRate, _vm.TotalRate);
        }

        [Test]
        public void Validate_FailsWithEmptyEmail()
        {
            _vm.ApproverEmail = "";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithNoAtSymbol()
        {
            _vm.ApproverEmail = "test.com";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidEmail()
        {
            _vm.ApproverEmail = "test@test.com";
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithPastStartDate()
        {
            _vm.StartDate = DateTime.MinValue;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithPastEndDate()
        {
            _vm.EndDate = DateTime.MinValue;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithEndDateBeforeStartDate()
        {
            _vm.StartDate = DateTime.Now.AddDays(1);
            _vm.EndDate = DateTime.Now;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidDates()
        {
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithZeroRate()
        {
            _vm.ContractorRate = 0;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithLessThanZeroRate()
        {
            _vm.ContractorRate = -100;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithGreatherThanZeroRate()
        {
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsNoContractTitle()
        {
            _vm.ContractTitle = "";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidContractTitle()
        {
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsNoSpecialization()
        {
            _vm.Specialization = new Specialization();
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidSpecialization()
        {
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }
    }
}
