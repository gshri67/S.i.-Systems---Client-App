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
    public class ContractStatusListTableViewModelTests
    {
        private ContractListTableViewModel _viewModel; 
        [SetUp]
        public void Setup()
        {
        }

        /*
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
        }*/
    }
}
