using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Tests.AccountExecutiveApp
{
    [TestFixture]
    public class JobServiceTests
    {
        private Mock<ISessionContext> _session;
        private Mock<IJobsRepository> _jobsRepo;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            _session = new Mock<ISessionContext>();
            _jobsRepo = new Mock<IJobsRepository>();

            _session.Setup(context => context.CurrentUser).Returns(new User
            {
                Id = UserId
            });
        }

        [Test]
        public void GetJobs_CallsGetJobsByAccountExecutiveId_WithCurrentUsersId()
        {
            _jobsRepo.Setup(repository => repository.GetJobsByAccountExecutiveId(It.IsAny<int>()));

            var service = new JobService(_jobsRepo.Object, _session.Object);
            service.GetJobs();

            _jobsRepo.Verify(repository => repository.GetJobsByAccountExecutiveId(UserId));
        }

        //todo: add a tests for GetJobDetailsById to test the logic within AssertCurrentUserHasPermissionsToViewJobDetails
    }
}
