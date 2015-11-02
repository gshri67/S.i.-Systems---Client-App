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
        #region FlowThru
        #region FlowThruCurrentContracts
        [Test]
        public void FlowThruCurrentContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FlowThruCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruCurrentContracts_NegativeValues_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruCurrentContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruCurrentContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruCurrentContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Current = expected
                },
                FullySourcedContracts = null,
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruCurrentContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FlowThruStartingContracts
        [Test]
        public void FlowThruStartingContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FlowThruStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruStartingContracts_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruStartingContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruStartingContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruStartingContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Starting = expected
                },
                FullySourcedContracts = null,
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruStartingContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FlowThruEndingContracts
        [Test]
        public void FlowThruEndingContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FlowThruEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruEndingContracts_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruEndingContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruEndingContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FlowThruEndingContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Ending = expected
                },
                FullySourcedContracts = null,
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FlowThruEndingContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion
        #endregion

        #region FullySourced
        #region FullySourcedCurrentContracts
        [Test]
        public void FullySourcedCurrentContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FullySourcedCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedCurrentContracts_NegativeValues_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedCurrentContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedCurrentContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedCurrentContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedCurrentContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = new ContractSummarySet
                {
                    Current = expected
                },
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedCurrentContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FullySourcedStartingContracts
        [Test]
        public void FullySourcedStartingContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FullySourcedStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedStartingContracts_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedStartingContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedStartingContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedStartingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedStartingContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = new ContractSummarySet
                {
                    Starting = expected
                },
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedStartingContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FullySourcedEndingContracts
        [Test]
        public void FullySourcedEndingContracts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.FullySourcedEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedEndingContracts_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedEndingContracts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedEndingContracts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedEndingContracts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void FullySourcedEndingContracts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = new ContractSummarySet
                {
                    Ending = expected
                },
                Jobs = null
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.FullySourcedEndingContracts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion
        #endregion

        #region Jobs
        #region AllJobs
        [Test]
        public void AllJobs_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.AllJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void AllJobs_NegativeValues_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.AllJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void AllJobs_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.AllJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void AllJobs_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.AllJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void AllJobs_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = null,
                Jobs = new JobsSummarySet
                {
                    All = expected
                }
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.AllJobs();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FullySourcedStartingContracts
        [Test]
        public void ProposedJobs_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.ProposedJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProposedJobs_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.ProposedJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProposedJobs_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.ProposedJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProposedJobs_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.ProposedJobs();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void ProposedJobs_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = null,
                Jobs = new JobsSummarySet
                {
                    Proposed = expected
                }
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.ProposedJobs();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion

        #region FullySourcedEndingContracts
        [Test]
        public void JobsWithCallouts_OnInitialization_ReturnsEmptyString()
        {
            _viewModel = new DashboardViewModel(_mockApi.Object);

            var actual = _viewModel.JobsWithCallouts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void JobsWithCallouts_WithInvalidSummary_ReturnsEmptyString()
        {
            SetupGetDashboardInfoReturnsNegativeValues();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.JobsWithCallouts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void JobsWithCallouts_WithNullReturnFromApi_ReturnsEmptyString()
        {
            SetupNullGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.JobsWithCallouts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void JobsWithCallouts_WithNullChildrenReturnFromApi_ReturnsEmptyString()
        {
            SetupNullChildrenGetDashboardInfoReturn();

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.JobsWithCallouts();

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void JobsWithCallouts_WithSuccessfulApiReturn_ReturnsIntegerToString()
        {
            const int expected = 10;
            _invalidDashboardInfo = new DashboardSummary
            {
                FlowThruContracts = null,
                FullySourcedContracts = null,
                Jobs = new JobsSummarySet { Callouts = expected}
            };
            _mockApi.Setup(api => api.getDashboardInfo()).Returns(Task.FromResult(_invalidDashboardInfo));
            _viewModel = new DashboardViewModel(_mockApi.Object);

            _viewModel.LoadDashboardInformation(null);
            var actual = _viewModel.JobsWithCallouts();

            Assert.AreEqual(expected.ToString(), actual);
        }
        #endregion
        #endregion
    }
}
