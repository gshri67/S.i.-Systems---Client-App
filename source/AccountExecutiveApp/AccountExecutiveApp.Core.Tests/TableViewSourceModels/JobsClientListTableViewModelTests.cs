using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.TableViewSourceModel;
using NUnit.Framework;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.Tests.TableViewSourceModels
{
    [TestFixture]
    public class JobsClientListTableViewModelTests
    {
        private JobsClientListTableViewModel _viewModel; 
        [SetUp]
        public void Setup()
        {
        }

        private static Job _jobCompanyOneDeveloperWithoutProposed = new Job
        {
            ClientName = "Company One",
            JobTitle = "Developer",
            HasCallout = false,
            IsProposed = false,
            IssueDate = DateTime.Now
        };

        private static Job _jobCompanyTwoDeveloperWithoutProposed = new Job
        {
            ClientName = "Company Two",
            JobTitle = "Developer",
            HasCallout = false,
            IsProposed = false,
            IssueDate = DateTime.Now
        };

        private static IEnumerable<Job> _jobsFromSameCompany = new List<Job>
        {
            _jobCompanyOneDeveloperWithoutProposed,
            _jobCompanyOneDeveloperWithoutProposed,
        }.AsEnumerable();

        private static IEnumerable<Job> _jobsFromTwoCompanies = new List<Job>
        {
            _jobCompanyOneDeveloperWithoutProposed,
            _jobCompanyTwoDeveloperWithoutProposed,
        }.AsEnumerable();
        
        [Test]
        public void NumberOfGroups_WhenJobsNull_ReturnsZero()
        {
            _viewModel = new JobsClientListTableViewModel(null);

            var actual = _viewModel.NumberOfGroups();

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void NumberOfGroups_MultipleJobsSameCompany_ReturnsOne()
        {
            _viewModel = new JobsClientListTableViewModel(_jobsFromSameCompany);
            
            var actual = _viewModel.NumberOfGroups();

            Assert.AreEqual(1, actual);
        }

        [Test]
        public void NumberOfGroups_MultipleJobsFromTwoCompanies_ReturnsTwo()
        {
            _viewModel = new JobsClientListTableViewModel(_jobsFromTwoCompanies);

            var actual = _viewModel.NumberOfGroups();

            Assert.AreEqual(2, actual);
        }

        [Test]
        public void IndexIsInBounds_WithNullJobsAndAnyInput_ReturnsFalse()
        {
            _viewModel = new JobsClientListTableViewModel(null);

            Assert.False(_viewModel.IndexIsInBounds(0));
            Assert.False(_viewModel.IndexIsInBounds(-1));
            Assert.False(_viewModel.IndexIsInBounds(1));
        }

        [Test]
        public void IndexIsInBounds_TwoCompanies_ZeroAndOneReturnTrue()
        {
            _viewModel = new JobsClientListTableViewModel(_jobsFromTwoCompanies);

            Assert.True(_viewModel.IndexIsInBounds(0));
            Assert.True(_viewModel.IndexIsInBounds(1));
        }

        [Test]
        public void IndexIsInBounds_TwoCompanies_NegativeOneAndTwoReturnFalse()
        {
            _viewModel = new JobsClientListTableViewModel(_jobsFromTwoCompanies);

            Assert.False(_viewModel.IndexIsInBounds(-1));
            Assert.False(_viewModel.IndexIsInBounds(2));
        }

        [Test]
        public void ClientNameByRowNumber_WithNullJobs_ReturnsEmptyString()
        {
            _viewModel = new JobsClientListTableViewModel(null);
            
            var actual = _viewModel.ClientNameByRowNumber(0);

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void JobsByRowNumber_WhenJobsNull_ReturnsEmptyEnumerable()
        {
            _viewModel = new JobsClientListTableViewModel(null);

            var actual = _viewModel.JobsByRowNumber(0);

            Assert.AreEqual(Enumerable.Empty<Job>(), actual);
        }
    }
}
