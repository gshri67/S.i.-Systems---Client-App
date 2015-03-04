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

        class FakeHttpHandler : DelegatingHandler
        {
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
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
                    case "/api/consultants/alumni/1":
                        json = "{\"id\":1}";
                        break;
                    case "/api/consultants/alumni":
                        json = "[{\"id\":1},{\"id\":2}]";
                        break;
                    default:
                        throw new NotImplementedException(string.Format("There is no fake data for this request: {0}", request.RequestUri.AbsolutePath));
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(json) };
            }
        }

        [SetUp]
        public void SetUp()
        {
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
        }

        [Test]
        public async void Login_ShouldGetAtAValidValidationResultFromLoginEndpoint()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };
            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            var result = await sut.Login("email@example.com", "password");

            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async void Logout_ShouldDeleteToken()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            await sut.Logout();

            this._mockTokenSource.Verify(t => t.Remove(), Times.Once);
        }

        [Test]
        public void GetConsultant_ShouldGetAConsultant()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            var result = sut.GetConsultant(1).Result;

            Assert.IsInstanceOf<Consultant>(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetConsultants_ShouldGetAListOfConsultants()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken());

            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            var result = sut.GetConsultantGroups(string.Empty).Result;

            Assert.IsAssignableFrom<ConsultantGroup[]>(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
