using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Tests.AccountExecutiveApp
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private Mock<IJobsRepository> _jobsRepoMock;
        private Mock<IContractsRepository> _contractsRepoMock;
        private Mock<ISessionContext> _sessionMock;
        
        [SetUp]
        public void Setup()
        {
            _jobsRepoMock = new Mock<IJobsRepository>();
            _contractsRepoMock = new Mock<IContractsRepository>();
            _sessionMock = new Mock<ISessionContext>();
        }

        [Test]
        public void GetDashboardSummary_ReturnsDashboardSummary()
        {
            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _sessionMock.Object);
            var expected = new DashboardSummary
            {
                FlowThruContracts = new ContractSummarySet
                {
                    Current = 55,
                    Starting = 17,
                    Ending = 12
                },
                FullySourcedContracts = new ContractSummarySet
                {
                    Current = 55,
                    Starting = 17,
                    Ending = 12
                },
                Jobs = new JobsSummarySet
                {
                    All = 18,
                    Proposed = 9,
                    Callouts = 6
                }
            };

            var summary = service.GetDashboardSummary();

            AssertSummariesAreEqual(expected, summary);
        }

        [Test]
        public void GetDashboardSummary_WhenAuthorized_CallsOnContractsRepository()
        {
            _contractsRepoMock.Setup(repository => repository.GetSummaryForDashboard());

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            _contractsRepoMock.VerifyAll();
        }

        private void AssertSummariesAreEqual(DashboardSummary expected, DashboardSummary actual)
        {
            Assert.AreEqual(expected.FullySourcedContracts.Current, actual.FullySourcedContracts.Current);
            Assert.AreEqual(expected.FullySourcedContracts.Starting, actual.FullySourcedContracts.Starting);
            Assert.AreEqual(expected.FullySourcedContracts.Ending, actual.FullySourcedContracts.Ending);

            Assert.AreEqual(expected.FlowThruContracts.Current, actual.FlowThruContracts.Current);
            Assert.AreEqual(expected.FlowThruContracts.Starting, actual.FlowThruContracts.Starting);
            Assert.AreEqual(expected.FlowThruContracts.Ending, actual.FlowThruContracts.Ending);

            Assert.AreEqual(expected.Jobs.All, actual.Jobs.All);
            Assert.AreEqual(expected.Jobs.Proposed, actual.Jobs.Proposed);
            Assert.AreEqual(expected.Jobs.Callouts, actual.Jobs.Callouts);
        }
    }
}
