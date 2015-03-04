﻿using System;
using System.Collections.Generic;
using ClientApp.Services;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Tests.ViewModels
{
    [TestFixture]
    public class OnboardViewModelTests
    {
        private OnboardViewModel _vm;
        private Mock<IContractService> _contractMock;

        [SetUp]
        public void SetUp()
        {
            _contractMock = new Mock<IContractService>();
            _vm = new OnboardViewModel(_contractMock.Object)
                  {
                      StartDate = DateTime.Now,
                      EndDate = DateTime.Now.AddDays(1),
                      TimesheetApprovalEmail = "test@test.com",
                      ContractApprovalEmail = "test@test.com",
                      ContractorRate = 100,
                      ContractTitle = "Senior Developer",
                      MspPercent = 0,
                      ServiceRate = 3
                  };
        }

        [Test]
        public void Consultant_SettingAlsoSetsLastRate()
        {
            var consultant = new Consultant();
            consultant.Contracts.Add(new Contract{EndDate = DateTime.MaxValue, Rate = 80});

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.LastContractRate);
        }

        [Test]
        public void Consultant_RateSetUsingLastContract()
        {
            var consultant = new Consultant();
            consultant.Contracts.Add(new Contract { EndDate = DateTime.MaxValue, Rate = 80 });
            consultant.Contracts.Add(new Contract { EndDate = DateTime.MinValue, Rate = 120 });

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.LastContractRate);
        }

        [Test]
        public void TotalRate_AddsContractorAndServiceRate()
        {
            _vm.ContractorRate = 80;
            Assert.AreEqual(80 + _vm.ServiceRate, _vm.TotalRate);
        }

        [Test]
        public void Validate_FailsWithEmptyEmail()
        {
            _vm.TimesheetApprovalEmail = "";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithNoAtSymbol()
        {
            _vm.TimesheetApprovalEmail = "test.com";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_FailsWithBadContractorEmail()
        {
            _vm.ContractApprovalEmail = "test@test.com@test.com";
            var result = _vm.Validate();
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PassesWithValidEmail()
        {
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
        public void Validate_PassesWithValidSpecialization()
        {
            var result = _vm.Validate();
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetRateFooter_EmptyWhenNoFees()
        {
            _vm.MspPercent = 0;
            _vm.ServiceRate = 0;
            Assert.AreEqual(string.Empty, _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_NoserviceWhenZero()
        {
            _vm.MspPercent = 2;
            _vm.ServiceRate = 0;
            Assert.AreEqual("+ MSP rate (2%) = $102", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_MspAndServiceWhenNotZero()
        {
            _vm.MspPercent = 2;
            Assert.AreEqual("+ MSP rate (2%) + Service Fee ($3/hr) = $105", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_NoMSPWhenZero()
        {
            Assert.AreEqual("+ Service Fee ($3/hr) = $103", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_MSPWhenNotZero()
        {
            _vm.MspPercent = 2;
            _vm.ServiceRate = 0;
            Assert.AreEqual("+ MSP rate (2%) = $102", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_TotalIsCorrect()
        {
            _vm.ContractorRate = 50;
            _vm.ServiceRate = 5;
            Assert.AreEqual("+ Service Fee ($5/hr) = $55", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_TotalIsTwoDecimals()
        {
            _vm.MspPercent = 3.3333m;
            _vm.ServiceRate = 0;
            Assert.AreEqual("+ MSP rate (3.3333%) = $103.33", _vm.GetRateFooter());
        }
    }
}