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
            var expected = new DashboardInfo
            {
                FS_curContracts = 55,
                FS_startingContracts = 17,
                FS_endingContracts = 12,
                FT_curContracts = 55,
                FT_startingContracts = 17,
                FT_endingContracts = 12,
                curJobs = 18,
                calloutJobs = 6,
                proposedJobs = 9
            };

            var summary = service.GetDashboardSummary();
            
            Assert.AreEqual(expected.FS_endingContracts, summary.FS_endingContracts);
            Assert.AreEqual(expected.FS_startingContracts, summary.FS_startingContracts);
            Assert.AreEqual(expected.FS_curContracts, summary.FS_curContracts);

            Assert.AreEqual(expected.FT_endingContracts, summary.FT_endingContracts);
            Assert.AreEqual(expected.FT_startingContracts, summary.FT_startingContracts);
            Assert.AreEqual(expected.FT_curContracts, summary.FT_curContracts);

            Assert.AreEqual(expected.curJobs, summary.curJobs);
            Assert.AreEqual(expected.proposedJobs, summary.proposedJobs);
            Assert.AreEqual(expected.calloutJobs, summary.calloutJobs);
        }
    }
}
