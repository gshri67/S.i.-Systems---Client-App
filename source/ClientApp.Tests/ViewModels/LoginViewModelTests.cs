using System;
using ClientApp.Core.ViewModels;
using ClientApp.Core;
using Moq;
using NUnit.Framework;

namespace SiSystems.ClientApp.Tests.ViewModels
{
    [TestFixture]
    public class LoginViewModelTests
    {
        private Mock<IMatchGuideApi> _apiMock;
        private LoginViewModel _vm;

        [SetUp]
        public void Setup()
        {
            _apiMock = new Mock<IMatchGuideApi>();
            _vm = new LoginViewModel(_apiMock.Object);
        }

        [Test]
        public void IsValidUserName_FailsNull()
        {
            Assert.IsFalse(_vm.IsValidUserName(null).IsValid);
        }

        [Test]
        public void IsValidUserName_FailsBlank()
        {
            Assert.IsFalse(_vm.IsValidUserName("").IsValid);
        }

        [Test]
        public void IsValidUserName_AcceptsFilled()
        {
            Assert.IsTrue(_vm.IsValidUserName("Test Name").IsValid);
        }

        [Test]
        public void IsValidUserName_ReturnsMessageOnFail()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_vm.IsValidUserName("").Message));
        }

        [Test]
        public void IsValidPassword_FailsNull()
        {
            Assert.IsFalse(_vm.IsValidPassword(null).IsValid);
        }

        [Test]
        public void IsValidPassword_FailsBlank()
        {
            Assert.IsFalse(_vm.IsValidPassword("").IsValid);
        }

        [Test]
        public void IsValidPassword_FailsTooShort()
        {
            Assert.IsFalse(_vm.IsValidPassword("aa").IsValid);
        }

        [Test]
        public void IsValidPassword_AcceptsGoodPass()
        {
            Assert.IsTrue(_vm.IsValidPassword("pass@word1").IsValid);
        }

        [Test]
        public void IsValidPassword_ReturnsMessageOnFail()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_vm.IsValidPassword("").Message));
        }

        [Test]
        public void UserHasReadLatestEula_PopulatesEulaVersions()
        {
            _vm.UserHasReadLatestEula("testuser", 1, null);
            Assert.IsNotNull(_vm.EulaVersions);
        }

        [Test]
        public void UserHasReadLatestEula_FalseIfNoRecord()
        {
            Assert.IsFalse(_vm.UserHasReadLatestEula("testuser", 1, null));
        }
        
        [Test]
        public void UserHasReadLatestEula_FalseIfNoUser()
        {
            Assert.IsFalse(_vm.UserHasReadLatestEula("testuser", 1, "{\"anotheruser\":1}"));
        }

        [Test]
        public void UserHasReadLatestEula_FalseIfNewVersion()
        {
            Assert.IsFalse(_vm.UserHasReadLatestEula("testuser", 2, "{\"testuser\":1}"));
        }

        [Test]
        public void UserHasReadLatestEula_TrueIfMatch()
        {
            Assert.IsTrue(_vm.UserHasReadLatestEula("testuser", 1, "{\"testuser\":1}"));
        }
    }
}
