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
    public class ContractorJobStatusListViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private ContractorJobStatusListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new ContractorJobStatusListViewModel( _mockApi.Object);
        }

        [Test]
        public void ContractorNameByRowNumber_EmptyConsultants_ReturnsEmptyString()
        {
            _viewModel.LoadContractors(null);

            Assert.AreEqual(string.Empty, _viewModel.ContractorNameByRowNumber(0));
        }

        [Test]
        public void ContractorNameByRowNumber_IndexOutOfBounds_ReturnsEmptyString()
        {
            _viewModel.LoadContractors(new List<Contractor>
            {
                new Contractor
                {
                    FirstName = "Bob"
                }
            });
            var oneMoreThanNumberOfConsultants = _viewModel.NumberOfContractors() + 1;

            Assert.AreEqual(string.Empty, _viewModel.ContractorNameByRowNumber(oneMoreThanNumberOfConsultants));
        }

        [Test]
        public void NumberofContractors_EmptyConsultants_ReturnsZero()
        {
            _viewModel.LoadContractors(null);

            Assert.AreEqual(0, _viewModel.NumberOfContractors());
        }

        [Test]
        public void NumberOfContractors_SingleContractor_ReturnsOne()
        {
            const int expectedNumberOfConsultants = 1;
            _viewModel.LoadContractors(new List<Contractor>
            {
                new Contractor
                {
                    FirstName = "Bob"
                }
            });

            Assert.AreEqual(expectedNumberOfConsultants, _viewModel.NumberOfContractors());
        }
    }
}
