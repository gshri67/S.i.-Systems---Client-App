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
    }
}
