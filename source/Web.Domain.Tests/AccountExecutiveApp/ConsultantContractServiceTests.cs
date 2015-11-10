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
        private Mock<ISessionContext> _sessionMock;

        [SetUp]
        public void Setup()
        {
            _contractsRepoMock = new Mock<IConsultantContractRepository>();
            _sessionMock = new Mock<ISessionContext>();
        }
        /*
        [Test]
        public void EndingContracts_WithMultipleContracts_DateIsWithinThirtyDaysOfEnding()
        {
            var testContracts = new List<ConsultantContract>();

            DateTime currentDate = new DateTime(2015, 11, 11);
            
            var contractEndingSoon = new ConsultantContract();
            contractEndingSoon.EndDate = new DateTime(2015, 11, 15);
            contractEndingSoon.StartDate = new DateTime(2015, 9, 15);
            testContracts.Add( contractEndingSoon );

            var contractStartingSoon = new ConsultantContract();
            contractStartingSoon.EndDate = new DateTime(2016, 3, 15);
            contractStartingSoon.StartDate = new DateTime(2015, 11, 15);
            testContracts.Add(contractStartingSoon);

            var activeContract = new ConsultantContract();
            activeContract.EndDate = new DateTime(2016, 2, 15);
            activeContract.StartDate = new DateTime(2015, 9, 15);
            testContracts.Add(activeContract);


            _contractsRepoMock.Setup(repository => repository.GetContracts())
                .Returns( testContracts.AsEnumerable() );

            var service = new ConsultantContractService(_contractsRepoMock.Object);

            var resultedContracts = service.GetContracts();

            int numEndingContracts = 0;
            foreach (var contract in resultedContracts)
            {
                //if contract is ending soon
                if ((contract.EndDate - currentDate).TotalDays <= 30)
                {
                    Assert.AreEqual( MatchGuideConstants.ConsultantContractStatusTypes.Ending, contract.StatusType );
                    numEndingContracts ++;
                }
            }

            Assert.AreEqual(1, numEndingContracts);
        }
        */
    }
}
