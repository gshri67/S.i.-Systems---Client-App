using System;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using ClientApp.Core;
using ClientApp.Core.ViewModels;
using System.Collections.Generic;

namespace SiSystems.ClientApp.Core.Tests.ViewModels
{
    [TestFixture]
    public class OnboardViewModelTests
    {
        private OnboardViewModel _vm;
        private Mock<IMatchGuideApi> _apiMock;

        [SetUp]
        public void SetUp()
        {
            _apiMock = new Mock<IMatchGuideApi>();
            CurrentUser.Email = "test@test.com";
            CurrentUser.FloThruFeePayment = MatchGuideConstants.FloThruFeePayment.ClientPays;
            CurrentUser.FloThruMspPayment = MatchGuideConstants.FloThruMspPayment.AddToBillRate;
            CurrentUser.ServiceFee = 3;
            CurrentUser.MspPercent = 0;
            _vm = new OnboardViewModel(_apiMock.Object)
                  {
                      StartDate = DateTime.Now,
                      EndDate = DateTime.Now.AddDays(1),
                      TimesheetApprovalEmail = "test@test.com",
                      ContractApprovalEmail = "test@test.com",
                      ContractorRate = 100,
                      ContractTitle = "Senior Developer"
                  };
        }

        #region Consultant
        [Test]
        public void Consultant_SettingAlsoSetsLastRate()
        {
            var consultant = new Consultant
            {
                Contracts = new List<Contract> { new Contract { EndDate = DateTime.MaxValue, Rate = 80 } }
            }; 

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.LastContractRate);
        }

        [Test]
        public void Consultant_RateSetUsingLastContract()
        {
            var consultant = new Consultant
            {
                Contracts = new List<Contract>
                {
                    new Contract { EndDate = DateTime.MaxValue, Rate = 80 },
                    new Contract { EndDate = DateTime.MinValue, Rate = 120 }
                }
            };

            _vm.Consultant = consultant;
            Assert.AreEqual(80, _vm.LastContractRate);
        }
        #endregion

        #region Total Rate
        [Test]
        public void TotalRate_ClientPaysBoth()
        {
            RateTestSetup(false, false);
            Assert.AreEqual(105, _vm.TotalRate);
        }

        [Test]
        public void TotalRate_ClientPaysService()
        {
            RateTestSetup(true, false);
            Assert.AreEqual(102, _vm.TotalRate);
        }

        [Test]
        public void TotalRate_ClientPaysMsp()
        {
            RateTestSetup(false, true);
            Assert.AreEqual(103, _vm.TotalRate);
        }

        [Test]
        public void TotalRate_ClientPaysNone()
        {
            RateTestSetup(true, true);
            Assert.AreEqual(100, _vm.TotalRate);
        }

        

        private void RateTestSetup(bool conPaysFee, bool conPaysMsp)
        {
            if (conPaysMsp)
                CurrentUser.FloThruMspPayment = MatchGuideConstants.FloThruMspPayment.DeductFromContractorPay;
            if (conPaysFee)
                CurrentUser.FloThruFeePayment = MatchGuideConstants.FloThruFeePayment.ContractorPays;

            CurrentUser.MspPercent = 2;

            _vm = new OnboardViewModel(_apiMock.Object)
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                TimesheetApprovalEmail = "test@test.com",
                ContractApprovalEmail = "test@test.com",
                ContractorRate = 100,
                ContractTitle = "Senior Developer"
            };
        }
        #endregion

        #region Validate
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
        public void Validate_FailsWithWrongDomain()
        {
            CurrentUser.Email = "bob@email.com";
            _vm.TimesheetApprovalEmail = "test@test.com";
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
        #endregion

        #region GetRateFooter
        [Test]
        public void GetRateFooter_EmptyWhenNoFees()
        {
            _vm.MspPercent = 0;
            _vm.ServiceFee = 0;
            Assert.AreEqual(string.Empty, _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_NoserviceWhenZero()
        {
            _vm.MspPercent = 2;
            _vm.ServiceFee = 0;
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
            _vm.ServiceFee = 0;
            Assert.AreEqual("+ MSP rate (2%) = $102", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_TotalIsCorrect()
        {
            _vm.ContractorRate = 50;
            _vm.ServiceFee = 5;
            Assert.AreEqual("+ Service Fee ($5/hr) = $55", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_TotalIsTwoDecimals()
        {
            _vm.MspPercent = 3.3333m;
            _vm.ServiceFee = 0;
            Assert.AreEqual("+ MSP rate (3.3333%) = $103.33", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_HidesMspIfContractorPays()
        {
            RateTestSetup(false, true);
            Assert.AreEqual("+ Service Fee ($3/hr) = $103", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_HidesServiceIfContractorPays()
        {
            RateTestSetup(true, false);
            Assert.AreEqual("+ MSP rate (2%) = $102", _vm.GetRateFooter());
        }

        [Test]
        public void GetRateFooter_HidesBothIfContractorPays()
        {
            RateTestSetup(true, true);
            Assert.AreEqual("", _vm.GetRateFooter());
        }
        #endregion
    }
}
