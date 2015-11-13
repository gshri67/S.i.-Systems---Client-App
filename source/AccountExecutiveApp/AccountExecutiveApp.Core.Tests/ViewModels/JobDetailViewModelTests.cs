using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Moq;
using NUnit.Framework;
using Shared.Core;

namespace AccountExecutiveApp.Core.Tests.ViewModels
{
    [TestFixture]
    public class JobDetailViewModelTests
    {
        private Mock<IMatchGuideApi> _apiMock;
        private JobDetailViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _apiMock = new Mock<IMatchGuideApi>();
        }

        [Test]
        public void JobDetailsProperties_OnInstantiation_ReturnsDefaultValues()
        {
            _viewModel = new JobDetailViewModel(_apiMock.Object);
            
            Assert.AreEqual(string.Empty, _viewModel.JobTitle);
            Assert.AreEqual(string.Empty, _viewModel.ClientContactName);
            Assert.AreEqual(string.Empty, _viewModel.DirectReportName);
            Assert.AreEqual(0, _viewModel.NumberOfShortlistedConsultants);
            Assert.AreEqual(0, _viewModel.NumberOfProposedContractors);
            Assert.AreEqual(0, _viewModel.NumberOfContractorsWithCallouts);
        }
    }
}
