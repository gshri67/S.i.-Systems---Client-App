using ClientApp.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.Core;
using Shared.Core.Platform;
using SiSystems.SharedModels;

namespace ClientApp.Core.Tests
{
    [TestFixture]
    public class MatchGuideApiTests
    {
        private Mock<ITokenStore> _mockTokenSource;
        private Mock<IActivityManager> _mockActivityManager;
        private Mock<IHttpMessageHandlerFactory> _mockHttpHandlerHelper;
        private Mock<IErrorSource> _mockErrorSource;
        private int NumRequests;

        class FakeHttpHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                string json;
                switch (request.RequestUri.AbsolutePath)
                {
                    case "/api/login":
                        json = "{\"access_token\": \"V9QKCstvkJ3XNmhn3iNwPnMQe7ecMacJFlBwA5ncSMPSE_E08eynYAUBSsMo4EmCzBsivAO3H3aeKTDXAibW2IQQmxAbl2m8IQgS2RWq4gzIIxm9sZGdCgt8KhFuuhCYkmGJzx5aQPggimpLs1E8PXrY82vA9ht9GWbaoQwE54RLwKQ9cBMWLsWsEyxB7LVI5DkoHmlDYHkBwvEAbowk3bHIYxEb0A_cTcEuvUeuGjKq4VW82I6UApPApYj2WEy2w-a8X9ybTVuswZ_w0JeRUVqMApiL9MwIY17JQuOpmyXL_yAMWACE4AKhl5GG1XRwG9d94BMpawkFi37dKQ_dREgOXX6pHR9Ohlh_HSxzyS8ALmeLfyra9H-dHE552Be_NOxlVg\",\"token_type\": \"bearer\", \"expires_in\": 1209599,\"userName\": \"email@example.com\",\r\n    \".issued\": \"Tue, 03 Mar 2015 23:10:16 GMT\", \".expires\": \"Tue, 17 Mar 2015 23:10:16 GMT\"}";
                        break;
                    case "/api/logout":
                        json = string.Empty;
                        break;
                    case "/api/consultants/1":
                        json = "{\"id\":1}";
                        break;
                    case "/api/consultants/alumni":
                        json = "[{\"id\":1},{\"id\":2}]";
                        break;
                    default:
                        throw new NotImplementedException(string.Format("There is no fake data for this request: {0}", request.RequestUri.AbsolutePath));
                }
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(json) });
            }
        }

        [SetUp]
        public void SetUp()
        {
            NumRequests = 1;
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
            _mockHttpHandlerHelper = new Mock<IHttpMessageHandlerFactory>();
            _mockErrorSource = new Mock<IErrorSource>();
            _mockHttpHandlerHelper.Setup(h => h.Get()).Returns(new FakeHttpHandler());
        }

        [TearDown]
        public void Verify()
        {
            _mockActivityManager.Verify(service => service.StartActivity(), Times.Exactly(NumRequests));
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Exactly(NumRequests));
        }

        [Test]
        public void ApiClient_ShouldGetBaseAddressFromType()
        {
            NumRequests = 0;
            var _sut = new MatchGuideApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            Assert.AreEqual(Settings.MatchGuideApiAddress, _sut.BaseAddress.AbsoluteUri);
        }

        [Test]
        public async void Login_ShouldGetAtAValidValidationResultFromLoginEndpoint()
        {
            const string username = "email@example.com";
            const string password = "password";

            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };
            _mockTokenSource.Setup(service => service.SaveUserName(username));
            _mockTokenSource.Setup(service => service.SaveToken(It.Is<OAuthToken>(token => token.Username == username)));

            var _sut = new MatchGuideApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Login(username, password);

            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.IsTrue(result.IsValid);
            _mockTokenSource.VerifyAll();
        }

        [Test]
        public async void Logout_ShouldDeleteToken()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var _sut = new MatchGuideApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.Logout();

            this._mockTokenSource.Verify(t => t.DeleteDeviceToken(), Times.Once);
        }

        [Test]
        public void GetConsultant_ShouldGetAConsultant()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var _sut = new MatchGuideApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = _sut.GetConsultant(1).Result;

            Assert.IsInstanceOf<Consultant>(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetConsultants_ShouldGetAListOfConsultants()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var _sut = new MatchGuideApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = _sut.GetAlumniConsultantGroups(string.Empty).Result;

            Assert.IsAssignableFrom<ConsultantGroup[]>(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
