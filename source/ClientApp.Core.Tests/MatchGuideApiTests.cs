using ClientApp.Core;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.Core.Tests
{
    [TestFixture]
    public class MatchGuideApiTests
    {
        private Mock<ITokenStore> _mockTokenSource;
        private Mock<IActivityManager> _mockActivityManager;
        private Mock<IHttpMessageHandlerFactory> _mockHttpHandlerHelper;
        private Mock<IErrorSource> _mockErrorSource;

        [SetUp]
        public void SetUp()
        {
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
            _mockHttpHandlerHelper = new Mock<IHttpMessageHandlerFactory>();
            _mockErrorSource = new Mock<IErrorSource>();
            _mockHttpHandlerHelper.Setup(h => h.Get()).Returns(new JsonBackedHandler("../../MatchGuideApiDataSource.json"));
        }

        [Test]
        public async void Login_ShouldGetAtAValidValidationResultFromLoginEndpoint()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = await _sut.Login("email@example.com", "password");

            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async void Logout_ShouldDeleteToken()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            await _sut.Logout();

            this._mockTokenSource.Verify(t => t.DeleteDeviceToken(), Times.Once);
        }

        [Test]
        public void GetConsultant_ShouldGetAConsultant()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = _sut.GetConsultant(10).Result;

            Assert.IsInstanceOf<Consultant>(result);
            Assert.AreEqual(10, result.Id);
        }

        [Test]
        public void GetConsultants_ShouldGetAListOfAlumniConsultants()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = _sut.GetAlumniConsultantGroups(string.Empty).Result;

            Assert.IsAssignableFrom<ConsultantGroup[]>(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetConsultants_ShouldGetAListOfActiveConsultants()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = _sut.GetActiveConsultantGroups(string.Empty).Result;

            Assert.IsAssignableFrom<ConsultantGroup[]>(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetEula_ShouldGetTheEula()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = _sut.GetMostRecentEula().Result;

            Assert.IsAssignableFrom<Eula>(result);
            Assert.AreEqual(3, result.Version);
        }

        [Test]
        public void GetClientDetails_ShouldGetTheClientDetails()
        {
            var mockHttpHandler = new Mock<JsonBackedHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<IMatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var _sut = new MatchGuideApi(apiClient);
            var result = _sut.GetClientDetails().Result;

            Assert.IsAssignableFrom<ClientAccountDetails>(result);
        }
    }
}
