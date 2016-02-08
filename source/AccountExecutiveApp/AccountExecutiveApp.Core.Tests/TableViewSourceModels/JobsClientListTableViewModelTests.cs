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

        [Test]
        public void NumberOfGroups_WhenJobsNull_ReturnsZero()
        {
            _viewModel = new JobsClientListTableViewModel(null);

            var actual = _viewModel.NumberOfGroups();

            Assert.AreEqual(0, actual);
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
        public void ClientNameByRowNumber_WithNullJobs_ReturnsEmptyString()
        {
            _viewModel = new JobsClientListTableViewModel(null);
            
            var actual = _viewModel.ClientNameByRowNumber(0);

            Assert.AreEqual(string.Empty, actual);
        }
    }
}
