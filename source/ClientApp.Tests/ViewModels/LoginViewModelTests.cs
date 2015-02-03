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
        [Test]
        public void IsValidUserName_FailsNull()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) {UserName = null, Password = "pass@word1"};
            Assert.IsFalse(vm.IsValidUserName());
        }

        [Test]
        public void IsValidUserName_FailsBlank()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "", Password = "pass@word1" };
            Assert.IsFalse(vm.IsValidUserName());
        }

        [Test]
        public void IsValidUserName_AcceptsFilled()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "TestName", Password = "pass@word1" };
            Assert.IsTrue(vm.IsValidUserName());
        }

        [Test]
        public void IsValidPassword_FailsNull()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "TestName", Password = null };
            Assert.IsFalse(vm.IsValidPassword());
        }

        [Test]
        public void IsValidPassword_FailsBlank()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "TestName", Password = "" };
            Assert.IsFalse(vm.IsValidPassword());
        }

        [Test]
        public void IsValidPassword_FailsTooShort()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "TestName", Password = "aa" };
            Assert.IsFalse(vm.IsValidPassword());
        }

        [Test]
        public void IsValidPassword_AcceptsGoodPass()
        {
            var mock = new Mock<ILoginService>();
            var vm = new LoginViewModel(mock.Object) { UserName = "TestName", Password = "pass@word1" };
            Assert.IsTrue(vm.IsValidPassword());
        }
    }
}
