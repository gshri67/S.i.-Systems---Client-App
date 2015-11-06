using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.Tests.ViewModels
{
    [TestFixture]
    public class JobsViewModelTests
    {
        private JobsViewModel _viewModel;
        private Mock<IMatchGuideApi> _mockApi;

        private IEnumerable<Job> _singleJobResult = new List<Job>
        {
            new Job
            {
                ClientName = "Company One",
                isProposed = true,
                hasCallout = true,
                issueDate = DateTime.Now,
                JobTitle = "Developer"
            }
        }.AsEnumerable();

        private static Job _companyOneDeveloperJob = new Job
        {
            ClientName = "Company One",
            isProposed = true,
            hasCallout = true,
            issueDate = DateTime.Now,
            JobTitle = "Developer"
        };

        private static Job _companyOneManagementJob = new Job
        {
            ClientName = "Company One",
            isProposed = true,
            hasCallout = true,
            issueDate = DateTime.Now,
            JobTitle = "Management"
        };

        private static Job _companyTwoManagementJob = new Job
        {
            ClientName = "Company Two",
            isProposed = true,
            hasCallout = true,
            issueDate = DateTime.Now,
            JobTitle = "Management"
        };

        private IEnumerable<Job> _multipleJobsWithSameClients = new List<Job>
        {
            _companyOneDeveloperJob,
            _companyOneManagementJob
        }.AsEnumerable();

        private IEnumerable<Job> _multipleJobsWithDifferentClients = new List<Job>
        {
            _companyOneDeveloperJob,
            _companyTwoManagementJob
        }.AsEnumerable();
        
        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
        }

        [Test]
        //Name of method _ state you're testing it in _ Expected results
        public void JobsByClient_WithValidJobs_ReturnsJobInGroup()
        {
            _mockApi.Setup(api => api.getJobs()).Returns(Task.FromResult(_singleJobResult));
            _viewModel = new JobsViewModel(_mockApi.Object);

            _viewModel.LoadJobs(null);

            var jobsByClient = _viewModel.JobsByClient();
            
            var expected = _singleJobResult.GroupBy(job => job.ClientName);

            Assert.AreEqual(expected.ElementAt(0).Key, jobsByClient.ElementAt(0).Key);


            Job expectedJob = expected.ElementAt(0).First();
            Job actualJob = jobsByClient.ElementAt(0).First();

            AssertJobsAreEqual(expectedJob, actualJob);
        }

        [Test]
        public void JobsByClient_BeforeLoadingJobs_ReturnsEmptyGrouping()
        {
            _viewModel = new JobsViewModel(_mockApi.Object);

            var jobsByClient = _viewModel.JobsByClient();
            var actualGrouping = jobsByClient;

            var expected = new List<IGrouping<string, Job>>();

            Assert.AreEqual(expected, actualGrouping);
        }

        [Test]
        public void JobsByClient_WithTwoJobsInSameCompany_GroupsBothJobsUnderOnceCompany()
        {
            _mockApi.Setup(api => api.getJobs()).Returns(Task.FromResult(_multipleJobsWithSameClients));
            _viewModel = new JobsViewModel(_mockApi.Object);

            _viewModel.LoadJobs(null);

            var jobsByClient = _viewModel.JobsByClient();

            Assert.AreEqual(1, jobsByClient.Count());
            Assert.AreEqual(2, jobsByClient.First().Count());
        }

        [Test]
        public void JobsByClient_WithJobsInDifferentCompanies_GroupsJobsUnderDifferentCompanies()
        {
            _mockApi.Setup(api => api.getJobs()).Returns(Task.FromResult(_multipleJobsWithDifferentClients));
            _viewModel = new JobsViewModel(_mockApi.Object);

            _viewModel.LoadJobs(null);

            var jobsByClient = _viewModel.JobsByClient();

            Assert.AreEqual(2, jobsByClient.Count());
            Assert.AreEqual(1, jobsByClient.First().Count());
            Assert.AreEqual(1, jobsByClient.ElementAt(1).Count());
        }

        private static void AssertJobsAreEqual(Job expectedJob, Job actualJob)
        {
            Assert.AreEqual(expectedJob.hasCallout, actualJob.hasCallout);
            Assert.AreEqual(expectedJob.isProposed, actualJob.isProposed);
            Assert.AreEqual(expectedJob.JobTitle, actualJob.JobTitle);
            Assert.AreEqual(expectedJob.issueDate, actualJob.issueDate);
        }
    }
}