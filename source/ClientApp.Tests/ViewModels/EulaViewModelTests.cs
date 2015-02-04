using System;
using System.Collections.Generic;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels;
using Moq;
using NUnit.Framework;

namespace SiSystems.ClientApp.Tests.ViewModels
{
    [TestFixture]
    public class EulaViewModelTests
    {
        [Test]
        public void AcceptEula_AddsUser()
        {
            var vm = new EulaViewModel(new Dictionary<string, int>());
            vm.AcceptEula("testuser", 1);

            Assert.AreEqual("{\"testuser\":1}", vm.GetUpdatedStorageString());
        }

        [Test]
        public void AcceptEula_UpdatesOldUser()
        {
            var dic = new Dictionary<string, int> {{"testuser", 1}};
            var vm = new EulaViewModel(dic);
            vm.AcceptEula("testuser", 2);

            Assert.AreEqual("{\"testuser\":2}", vm.GetUpdatedStorageString());
        }

        [Test]
        public void AcceptEula_LeavesOtherUsers()
        {
            var dic = new Dictionary<string, int> { { "testuser", 1 } };
            var vm = new EulaViewModel(dic);
            vm.AcceptEula("newUser", 2);

            Assert.AreEqual("{\"testuser\":1,\"newUser\":2}", vm.GetUpdatedStorageString());
        }
    }
}
