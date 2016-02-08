using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.Tests.ViewModels
{
    [TestFixture]
    public class ContractDetailsViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private ContractDetailsViewModel _viewModel;

        private const string Zero = "0";

        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new ContractDetailsViewModel(_mockApi.Object);
        }

        private static readonly ContractSummarySet ContractsSummarySetWithNegativeValues = new ContractSummarySet
        {
            Current = -1,
            Ending = -1,
            Starting = -1
        };


        [Test]
        public void ContractProperties_WithNullReturnFromApi_ReturnZeroOrEmpty()
        {
            SetupNullGetContractDetailsReturn();

            _viewModel.LoadContractDetails(0);

            Assert.AreEqual(string.Empty, _viewModel.CompanyName);
            Assert.AreEqual(string.Empty, _viewModel.ConsultantsFullName);
            Assert.AreEqual(string.Empty, _viewModel.ContractPeriod);//date time objects cant be null
            Assert.AreEqual(string.Empty, _viewModel.FormattedStartDate);
            Assert.AreEqual(string.Empty, _viewModel.FormattedEndDate);
            Assert.AreEqual(string.Empty, _viewModel.ContractTitle);
            Assert.AreEqual(string.Empty, _viewModel.FormattedClientAndStatus);
            Assert.AreEqual(0, float.Parse( _viewModel.FormattedGrossMargin, NumberStyles.Currency ));
            //Assert.AreEqual(0, float.Parse(_viewModel.FormattedMarkup, NumberStyles.Currency));
            Assert.AreEqual(0, float.Parse(_viewModel.FormattedPayRate, NumberStyles.Currency));
            Assert.AreEqual(0, float.Parse(_viewModel.FormattedBillRate, NumberStyles.Currency));
        }


        private void SetupNullGetContractDetailsReturn()
        {
            _mockApi.Setup(api => api.GetContractDetailsById(0)).Returns(Task.FromResult(( ConsultantContract )null));
            _viewModel = new ContractDetailsViewModel(_mockApi.Object);
        }
    }
}
