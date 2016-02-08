using System;
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
    public class DashboardViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private DashboardViewModel _viewModel;

        private const string Zero = "0";

        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new DashboardViewModel(_mockApi.Object);
        }

        private void SetupGetDashboardInfoReturnsNegativeValues()
        {
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = ContractsSummarySetWithNegativeValues,
                FullySourcedContracts = ContractsSummarySetWithNegativeValues,
                Jobs = JobsSummaryWithNegativeValues
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);
        }

        private void SetupNullGetDashboardInfoReturn()
        {
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult((DashboardSummary)null));
            _viewModel = new DashboardViewModel(_mockApi.Object);
        }

        private void SetupNullChildrenGetDashboardInfoReturn()
        {
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = null,
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);
        }

        private static readonly ContractSummarySet ContractsSummarySetWithNegativeValues = new ContractSummarySet
        {
            Current = -1,
            Ending = -1,
            Starting = -1
        };

        private static readonly JobsSummarySet JobsSummaryWithNegativeValues = new JobsSummarySet
        {
            All = -1,
            Callouts = -1,
            Proposed = -1
        };

        private static DashboardSummary _invalidDashboardInfo = new DashboardSummary
        {
            FlowThruContracts = ContractsSummarySetWithNegativeValues,
            FullySourcedContracts = ContractsSummarySetWithNegativeValues,
            Jobs = JobsSummaryWithNegativeValues
        };
        
        [Test]
        public void DashboardProperties_OnInitialization_ReturnsZero()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            Assert.AreEqual(Zero, _viewModel.StartingFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFloThruContracts);

            Assert.AreEqual(Zero, _viewModel.StartingFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFullySourcedContracts);

            Assert.AreEqual(Zero, _viewModel.AllJobs);
            Assert.AreEqual(Zero, _viewModel.ProposedJobs);
            Assert.AreEqual(Zero, _viewModel.JobsWithCallouts);
        }

        [Test]
        public async Task DashboardProperties_WithNegativeValues_ReturnsZero()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            await _viewModel.LoadDashboardInformation();

            Assert.AreEqual(Zero, _viewModel.StartingFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFloThruContracts);

            Assert.AreEqual(Zero, _viewModel.StartingFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFullySourcedContracts);

            Assert.AreEqual(Zero, _viewModel.AllJobs);
            Assert.AreEqual(Zero, _viewModel.ProposedJobs);
            Assert.AreEqual(Zero, _viewModel.JobsWithCallouts);
        }

        [Test]
        public void DashboardProperties_WithNullReturnFromApi_ReturnZero()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation();

            Assert.AreEqual(Zero, _viewModel.StartingFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFloThruContracts);

            Assert.AreEqual(Zero, _viewModel.StartingFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFullySourcedContracts);

            Assert.AreEqual(Zero, _viewModel.AllJobs);
            Assert.AreEqual(Zero, _viewModel.ProposedJobs);
            Assert.AreEqual(Zero, _viewModel.JobsWithCallouts);
        }

        [Test]
        public void DashboardProperties_WithNullChildrenReturnFromApi_ReturnZero()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation();

            Assert.AreEqual(Zero, _viewModel.StartingFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFloThruContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFloThruContracts);

            Assert.AreEqual(Zero, _viewModel.StartingFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.CurrentFullySourcedContracts);
            Assert.AreEqual(Zero, _viewModel.EndingFullySourcedContracts);

            Assert.AreEqual(Zero, _viewModel.AllJobs);
            Assert.AreEqual(Zero, _viewModel.ProposedJobs);
            Assert.AreEqual(Zero, _viewModel.JobsWithCallouts);
        }

        [Test]
        public void FlowThruCurrentContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            var validDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Starting = expected,
                    Current = expected,
                    Ending = expected
                },
                FullySourcedContracts = new ContractSummarySet
                {
                    Starting = expected,
                    Current = expected,
                    Ending = expected
                },
                Jobs = new JobsSummarySet
                {
                    All = expected,
                    Proposed = expected,
                    Callouts = expected
                }
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(validDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation();
            
            Assert.AreEqual(expected.ToString(), _viewModel.StartingFloThruContracts);
            Assert.AreEqual(expected.ToString(), _viewModel.CurrentFloThruContracts);
            Assert.AreEqual(expected.ToString(), _viewModel.EndingFloThruContracts);

            Assert.AreEqual(expected.ToString(), _viewModel.StartingFullySourcedContracts);
            Assert.AreEqual(expected.ToString(), _viewModel.CurrentFullySourcedContracts);
            Assert.AreEqual(expected.ToString(), _viewModel.EndingFullySourcedContracts);

            Assert.AreEqual(expected.ToString(), _viewModel.AllJobs);
            Assert.AreEqual(expected.ToString(), _viewModel.ProposedJobs);
            Assert.AreEqual(expected.ToString(), _viewModel.JobsWithCallouts);
        }

        
    }
}
