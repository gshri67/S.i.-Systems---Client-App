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
        private Mock<IUserContactRepository> _userRepo;
        
        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            _session = new Mock<ISessionContext>();
            _jobsRepo = new Mock<IJobsRepository>();
            _userRepo = new Mock<IUserContactRepository>();

            _session.Setup(context => context.CurrentUser).Returns(new User
            {
                Id = UserId
            });
        }

        [Test]
        public void GetJobs_CallsGetJobsByAccountExecutiveId_WithCurrentUsersId()
        {
            const int clientId = 1;
            _jobsRepo.Setup(repository => repository.GetJobsByClientIdAndAccountExecutiveId(It.IsAny<int>(), It.IsAny<int>()));

            var service = new JobService(_jobsRepo.Object, _userRepo.Object, _session.Object);
            service.GetJobsByClientId(clientId);

            _jobsRepo.Verify(repository => repository.GetJobsByClientIdAndAccountExecutiveId(clientId, _session.Object.CurrentUser.Id));
        }
    }
}
