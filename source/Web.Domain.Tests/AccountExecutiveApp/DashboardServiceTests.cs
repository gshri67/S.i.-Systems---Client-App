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
            _contractsRepoMock.Setup(repo => repo.ActiveFloThruContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int>{1,2,3,4,5});
            _contractsRepoMock.Setup(repo => repo.StartingFloThruContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            _contractsRepoMock.Setup(repo => repo.EndingFloThruContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int> { 1, 2 });

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object,_timesheetsRepoMock.Object ,_sessionMock.Object);

            var summary = service.GetDashboardSummary();

            Assert.AreEqual(5, summary.FlowThruContracts.Current);
            Assert.AreEqual(9, summary.FlowThruContracts.Starting);
            Assert.AreEqual(2, summary.FlowThruContracts.Ending);
        }

        [Test]
        public void GetDashboardSummary_CallsFullySourcedSummary_PopulatesFullySourcedValue()
        {
            _contractsRepoMock.Setup(repo => repo.ActiveFullySourcedContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int> { 1, 2, 3, 4, 5 });
            _contractsRepoMock.Setup(repo => repo.StartingFullySourcedContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            _contractsRepoMock.Setup(repo => repo.EndingFullySourcedContractsForAccountExecutive(It.IsAny<int>()))
                .Returns(new List<int> { 1, 2 });

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            Assert.AreEqual(5, summary.FullySourcedContracts.Current);
            Assert.AreEqual(9, summary.FullySourcedContracts.Starting);
            Assert.AreEqual(2, summary.FullySourcedContracts.Ending);
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
        public void GetDashboardSummary_CallsGetJobsRepository()
        {
            _jobsRepoMock.Setup(repository => repository.GetSummaryCountsByAccountExecutiveId(It.IsAny<int>()));

            var service = new DashboardService(_jobsRepoMock.Object, _contractsRepoMock.Object, _timesheetsRepoMock.Object, _sessionMock.Object);

            var summary = service.GetDashboardSummary();

            _contractsRepoMock.VerifyAll();
        }
    }
}
