using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Tests.AccountExecutiveApp
{
    [TestFixture]
    public class ConsultantContractServiceTests
    {
        private Mock<IConsultantContractRepository> _contractsRepoMock;
        private Mock<IContractorRepository> _contractorRepoMock;
        private Mock<IUserContactRepository> _userContactRepoMock;
        private Mock<IDateTimeService> _dateTimeMock;
        private Mock<ISessionContext> _session;

        private static readonly DateTime OneWeekFromNow = DateTime.UtcNow.AddDays(7);
        private static readonly DateTime TwoMonthsFromNow = DateTime.UtcNow.AddMonths(2);

        private static readonly DateTime TwoMonthsAgo = DateTime.UtcNow.AddMonths(-2);
        private static readonly DateTime TwoWeeksFromNow = DateTime.UtcNow.AddDays(14);

        [SetUp]
        public void Setup()
        {
            _contractsRepoMock = new Mock<IConsultantContractRepository>();
            _contractorRepoMock = new Mock<IContractorRepository>();
            _userContactRepoMock = new Mock<IUserContactRepository>();

            _dateTimeMock = new Mock<IDateTimeService>();
            _dateTimeMock.Setup(timeService => timeService.DateIsWithinNextThirtyDays(OneWeekFromNow))
                .Returns(true);
            _dateTimeMock.Setup(timeService => timeService.DateIsWithinNextThirtyDays(TwoMonthsAgo))
                .Returns(false);

            _session = new Mock<ISessionContext>();
            _session.Setup(context => context.CurrentUser).Returns(new User
            {
                Id = 1
            });
        }

        [Test]
        public void ContractStatusTypeForStartDateAndEndDate_EndingContract_ReturnsEndingStatus()
        {
            var contract = new ConsultantContractSummary
            {
                StartDate = TwoMonthsAgo, 
                EndDate = OneWeekFromNow
            };

            var service = new ConsultantContractService(_contractsRepoMock.Object, _contractorRepoMock.Object, _userContactRepoMock.Object, _dateTimeMock.Object, _session.Object);

            var actual = service.ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);

            Assert.AreEqual(ContractStatusType.Ending, actual);
        }

        [Test]
        public void ContractStatusTypeForStartDateAndEndDate_StartingContract_ReturnsStartingStatus()
        {
            var contract = new ConsultantContractSummary
            {
                StartDate = OneWeekFromNow,
                EndDate = TwoMonthsFromNow
            };

            var service = new ConsultantContractService(_contractsRepoMock.Object, _contractorRepoMock.Object, _userContactRepoMock.Object, _dateTimeMock.Object, _session.Object);

            var actual = service.ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);

            Assert.AreEqual(ContractStatusType.Starting, actual);
        }

        [Test]
        public void ContractStatusTypeForStartDateAndEndDate_StartingAndEndingContract_ReturnsStartingStatus()
        {
            var contract = new ConsultantContractSummary
            {
                StartDate = OneWeekFromNow,
                EndDate = TwoWeeksFromNow
            };

            var service = new ConsultantContractService(_contractsRepoMock.Object, _contractorRepoMock.Object, _userContactRepoMock.Object, _dateTimeMock.Object, _session.Object); ;

            var actual = service.ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);

            Assert.AreEqual(ContractStatusType.Starting, actual);
        }

        [Test]
        public void ContractStatusTypeForStartDateAndEndDate_PastContract_ReturnsActiveStatus()
        {
            var contract = new ConsultantContractSummary
            {
                StartDate = TwoMonthsAgo,
                EndDate = TwoMonthsAgo
            };

            var service = new ConsultantContractService(_contractsRepoMock.Object, _contractorRepoMock.Object, _userContactRepoMock.Object, _dateTimeMock.Object, _session.Object);

            var actual = service.ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);
            //todo: Is Active what we would actually want these to show as? Should there be another status?
            Assert.AreEqual(ContractStatusType.Active, actual);
        }

        [Test]
        public void ContractStatusTypeForStartDateAndEndDate_FutureContract_ReturnsActiveStatus()
        {
            var contract = new ConsultantContractSummary
            {
                StartDate = TwoMonthsFromNow,
                EndDate = TwoMonthsFromNow
            };

            var service = new ConsultantContractService(_contractsRepoMock.Object, _contractorRepoMock.Object, _userContactRepoMock.Object, _dateTimeMock.Object, _session.Object);

            var actual = service.ContractStatusTypeForStartDateAndEndDate(contract.StartDate, contract.EndDate);

            //todo: Is Active what we would actually want these to show as? Should there be another status?
            Assert.AreEqual(ContractStatusType.Active, actual);
        }
    }
}
