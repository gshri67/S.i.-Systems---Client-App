using System;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels;
using Moq;
using NUnit.Framework;

namespace SiSystems.ClientApp.Tests.ViewModels
{
    [TestFixture]
    public class LoginViewModelTests
    {
        private Mock<ILoginService> _loginMock;
        private Mock<IEulaService> _eulaMock;
        private Mock<IClientDetailsService> _clientDetailsMock;
        private LoginViewModel _vm;

        [SetUp]
        public void Setup()
        {
            _loginMock = new Mock<ILoginService>();
            _eulaMock = new Mock<IEulaService>();
            _clientDetailsMock = new Mock<IClientDetailsService>();
            _vm = new LoginViewModel(_loginMock.Object, _eulaMock.Object, _clientDetailsMock.Object);
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
