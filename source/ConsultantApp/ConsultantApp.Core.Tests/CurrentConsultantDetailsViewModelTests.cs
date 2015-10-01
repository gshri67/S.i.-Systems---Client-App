using System;
using System.Threading.Tasks;
using ConsultantApp.Core.ViewModels;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.Tests
{
    [TestFixture]
    public class CurrentConsultantDetailsViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private CurrentConsultantDetailsViewModel _viewModel;
        
        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new CurrentConsultantDetailsViewModel(_mockApi.Object);
        }

        [Test]
        public void HasCorporationName_EmptyConsultantDetails_ReturnsFalse()
        {
            var result = _viewModel.HasCorporationName();
            
            Assert.IsFalse(result);
        }

        [Test]
        public void LoadConsultantDetails_GetCurrentUserConsultantDetails()
        {
            _mockApi.Setup(api => api.GetCurrentUserConsultantDetails());
            
            _viewModel.LoadConsultantDetails();

            _mockApi.VerifyAll();
        }

        [Test]
        public void HasCorporationName_EmptyCorporationName_ReturnsFalse()
        {
            _mockApi.Setup(api => api.GetCurrentUserConsultantDetails())
                    .Returns(Task.FromResult(new ConsultantDetails
                    {
                        CorporationName = string.Empty
                    }
                )
            );

            _viewModel.LoadConsultantDetails();
            var result = _viewModel.HasCorporationName();

            Assert.IsFalse(result);
        }

        [Test]
        public void HasCorporationName_WithValidCorporationName_ReturnsTrue()
        {
            const string validCorporationName = "Valid Corporation Name";
            _mockApi.Setup(api => api.GetCurrentUserConsultantDetails())
                    .Returns(Task.FromResult(new ConsultantDetails
                    {
                        CorporationName = validCorporationName
                    }
                )
            );

            _viewModel.LoadConsultantDetails();
            var result = _viewModel.HasCorporationName();
            
            Assert.IsTrue(result);
        }

        [Test]
        public void ConsultantCorporationName_WithValidCorporationName_ReturnsCorporationName()
        {
            const string validCorporationName = "Valid Corporation Name";
            _mockApi.Setup(api => api.GetCurrentUserConsultantDetails())
                    .Returns(Task.FromResult(new ConsultantDetails
                    {
                        CorporationName = validCorporationName
                    }
                )
            );

            _viewModel.LoadConsultantDetails();

            var result = _viewModel.ConsultantCorporationName();

            Assert.AreEqual(validCorporationName, result);
        }
    }
}
