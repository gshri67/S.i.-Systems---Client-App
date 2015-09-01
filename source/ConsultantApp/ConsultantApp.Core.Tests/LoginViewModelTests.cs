using ConsultantApp.Core.ViewModels;
using Moq;
using NUnit.Framework;
using Shared.Core;

namespace ConsultantApp.Core.Tests
{
    [TestFixture]
    public class LoginViewModelTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private LoginViewModel _viewModel;
        
        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new LoginViewModel(_mockApi.Object);
        }

        [Test]
        public void IsValidPassword_WithEmptyPassword_ReturnsNegativeValidationResult()
        {
            var result = _viewModel.IsValidPassword(string.Empty);

            Assert.False(result.IsValid);
        }

        [Test]
        public void IsValidPassword_WithThreeCharacters_ReturnsNegativeValidationResult()
        {
            const string shortPassword = "123";

            var result = _viewModel.IsValidPassword(shortPassword);
            
            Assert.False(result.IsValid);
        }

        [Test]
        public void IsValidPassword_WithMoreThanThreeCharacters_ReturnsPositiveValidationResult()
        {
            const string longPassword = "123456789";

            var result = _viewModel.IsValidPassword(longPassword);

            Assert.True(result.IsValid);
        }

        [Test]
        public void IsValidPassword_WithNullPassword_ReturnsNegativeValidationResult()
        {
            var result = _viewModel.IsValidPassword(null);

            Assert.False(result.IsValid);
        }
    }
}
