using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultantApp.Core.ViewModels;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.Tests
{
    [TestFixture]
    public class ActiveTimesheetViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private ActiveTimesheetViewModel _viewModel;
        
        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new ActiveTimesheetViewModel(_mockApi.Object);
        }

        [Test]
        public void PayPeriods_NullReturnedFromApi_ReturnsEmptyEnumerable()
        {
            _mockApi.Setup(api => api.GetPayPeriodSummaries())
                    .Returns(Task.FromResult((IEnumerable<PayPeriod>)null));

            _viewModel.LoadPayPeriods();

            var expected = Enumerable.Empty<PayPeriod>();

            Assert.AreEqual(expected, _viewModel.PayPeriods);
        }

        [Test]
        public void UserHasPayPeriods_BeforeLoadingPayPeriods_ReturnsFalse()
        {
            var result = _viewModel.UserHasPayPeriods();

            Assert.IsFalse(result);
        }

        [Test]
        public void UserHasPayPeriods_WithEmptyPayPeriods_ReturnsFalse()
        {
            _mockApi.Setup(api => api.GetPayPeriodSummaries())
                    .Returns(Task.FromResult((IEnumerable<PayPeriod>)null));

            _viewModel.LoadPayPeriods();

            var result = _viewModel.UserHasPayPeriods();

            Assert.IsFalse(result);
        }

        [Test]
        public void UserHasPayPeriods_WithNonEmptyPayPeriods_ReturnsTrue()
        {
            _mockApi.Setup(api => api.GetPayPeriodSummaries())
                    .Returns(Task.FromResult(new List<PayPeriod>
                    {
                        new PayPeriod()
                    }.AsEnumerable()
                )
            );

            _viewModel.LoadPayPeriods();

            var result = _viewModel.UserHasPayPeriods();

            Assert.IsTrue(result);
        }
    }
}
