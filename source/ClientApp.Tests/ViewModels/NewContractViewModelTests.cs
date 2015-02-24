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
            _vm = new NewContractViewModel();
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
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithNoAtSymbol()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidEmail()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithPastStartDate()
        {
            _vm.StartDate = DateTime.MinValue;
            _vm.EndDate = DateTime.Now;
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithPastEndDate()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.MinValue;
            _vm.ApproverEmail = "test@test.com";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithEndDateBeforeStartDate()
        {
            _vm.StartDate = DateTime.Now.AddDays(1);
            _vm.EndDate = DateTime.Now;
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidDates()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithZeroRate()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 0;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithLessThanZeroRate()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 0;
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithGreatherThanZeroRate()
        {
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now.AddDays(1);
            _vm.ApproverEmail = "test@test.com";
            _vm.ContractorRate = 100;
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }
    }
}
