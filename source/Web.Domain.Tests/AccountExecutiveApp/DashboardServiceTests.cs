using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Tests.AccountExecutiveApp
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private Mock<IJobsRepository> _jobsRepoMock;
        private Mock<IConsultantContractRepository> _contractsRepoMock;
        private Mock<ITimesheetRepository> _timesheetsRepoMock;
        private Mock<ISessionContext> _sessionMock;
        
        [SetUp]
        public void Setup()
        {
            _jobsRepoMock = new Mock<IJobsRepository>();
            _timesheetsRepoMock = new Mock<ITimesheetRepository>();
            _contractsRepoMock = new Mock<IConsultantContractRepository>();
            _sessionMock = new Mock<ISessionContext>();
            _sessionMock.Setup(context => context.CurrentUser).Returns(new User
            {
                Id = 1
            });
        }

        [Test]
        public void GetDashboardSummary_CallsFlowThruSummary_PopulatesFlowThruValue()
        {
            var contractSummaryToReturn = new ContractSummarySet
            {
                Current = 55,
                Starting = 17,
                Ending = 12
            };
            _contractsRepoMock.Setup(repository => repository.GetFloThruSummaryByAccountExecutiveId(It.IsAny<int>()))
                .Returns(contractSummaryToReturn);

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object,_timesheetsRepoMock.Object ,_sessionMock.Object);

            var summary = service.GetDashboardSummary();

            Assert.AreEqual(contractSummaryToReturn.Current, summary.FlowThruContracts.Current);
            Assert.AreEqual(contractSummaryToReturn.Starting, summary.FlowThruContracts.Starting);
            Assert.AreEqual(contractSummaryToReturn.Ending, summary.FlowThruContracts.Ending);
        }

        [Test]
        public void GetDashboardSummary_CallsFullySourcedSummary_PopulatesFullySourcedValue()
        {
            var contractSummaryToReturn = new ContractSummarySet
            {
                Current = 30,
                Starting = 15,
                Ending = 10
            };
            _contractsRepoMock.Setup(repository => repository.GetFullySourcedSummaryByAccountExecutiveId(It.IsAny<int>()))
                .Returns(contractSummaryToReturn);

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            Assert.AreEqual(contractSummaryToReturn.Current, summary.FullySourcedContracts.Current);
            Assert.AreEqual(contractSummaryToReturn.Starting, summary.FullySourcedContracts.Starting);
            Assert.AreEqual(contractSummaryToReturn.Ending, summary.FullySourcedContracts.Ending);
        }

        [Test]
        public void GetDashboardSummary_CallsJobsSummary_PopulatesJobsValue()
        {
            var jobsToReturn = new JobsSummarySet
            {
                All = 18,
                Proposed = 9,
                Callouts = 6
            };
            _jobsRepoMock.Setup(repository => repository.GetSummaryCountsByAccountExecutiveId(It.IsAny<int>())).Returns(jobsToReturn);

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            Assert.AreEqual(jobsToReturn.All, summary.Jobs.All);
            Assert.AreEqual(jobsToReturn.Proposed, summary.Jobs.Proposed);
            Assert.AreEqual(jobsToReturn.Callouts, summary.Jobs.Callouts);
        }

        [Test]
        public void GetDashboardSummary_CallsGetFlowThruSummary()
        {
            _contractsRepoMock.Setup(repository => repository.GetFloThruSummaryByAccountExecutiveId(It.IsAny<int>()));

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            _contractsRepoMock.VerifyAll();
        }

        [Test]
        public void GetDashboardSummary_CallsGetFullySourcedSummary()
        {
            _contractsRepoMock.Setup(repository => repository.GetFullySourcedSummaryByAccountExecutiveId(It.IsAny<int>()));

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            _contractsRepoMock.VerifyAll();
        }

        [Test]
        public void GetDashboardSummary_CallsGetJobsRepository()
        {
            _jobsRepoMock.Setup(repository => repository.GetSummaryCountsByAccountExecutiveId(It.IsAny<int>()));

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            _contractsRepoMock.VerifyAll();
        }
    }
}
