using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

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
            Assert.AreEqual(0, _viewModel.NumberOfShortlistedConsultants);
            Assert.AreEqual(0, _viewModel.NumberOfProposedContractors);
            Assert.AreEqual(0, _viewModel.NumberOfContractorsWithCallouts);
        }

        [Test]
        public async void LoadJob_WithValidJob_CallsMatchguideApiWithJobId()
        {
            const int id = 1;
            _viewModel = new JobDetailViewModel(_apiMock.Object);

            await _viewModel.LoadJobWithJobID(id);

            _apiMock.Verify(api => api.GetJobWithJobId(id));
        }

        [Test]
        public async void TaskStatus_AfterSuccessfullJobLoad_ReturnsRanToCompletion()
        {
            _apiMock.Setup(api => api.GetJobDetails(It.IsAny<int>())).ReturnsAsync(new JobDetails());
            const int id = 1;
            _viewModel = new JobDetailViewModel(_apiMock.Object);

            var task = _viewModel.LoadJobWithJobID(id);
            await task;

            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
        }
    }
}
