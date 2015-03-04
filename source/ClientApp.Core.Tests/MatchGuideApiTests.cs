using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using System;
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
                switch (request.RequestUri.AbsolutePath)
                {
                    case "/api/login":
                        const string jsonToken = "{\"access_token\": \"V9QKCstvkJ3XNmhn3iNwPnMQe7ecMacJFlBwA5ncSMPSE_E08eynYAUBSsMo4EmCzBsivAO3H3aeKTDXAibW2IQQmxAbl2m8IQgS2RWq4gzIIxm9sZGdCgt8KhFuuhCYkmGJzx5aQPggimpLs1E8PXrY82vA9ht9GWbaoQwE54RLwKQ9cBMWLsWsEyxB7LVI5DkoHmlDYHkBwvEAbowk3bHIYxEb0A_cTcEuvUeuGjKq4VW82I6UApPApYj2WEy2w-a8X9ybTVuswZ_w0JeRUVqMApiL9MwIY17JQuOpmyXL_yAMWACE4AKhl5GG1XRwG9d94BMpawkFi37dKQ_dREgOXX6pHR9Ohlh_HSxzyS8ALmeLfyra9H-dHE552Be_NOxlVg\",\"token_type\": \"bearer\", \"expires_in\": 1209599,\"userName\": \"email@example.com\",\r\n    \".issued\": \"Tue, 03 Mar 2015 23:10:16 GMT\", \".expires\": \"Tue, 17 Mar 2015 23:10:16 GMT\"}";
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(jsonToken) };
                    case "/api/consultant/1":
                        const string json = "{\"id\":1}";
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(json) };
                    default:
                        throw new NotImplementedException(string.Format("There is no fake data for this request: {0}", request.RequestUri.AbsolutePath));
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
        }

        [Test]
        public void Login_ShouldGetAtAValidValidationResultFromLoginEndpoint()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };
            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            var result = sut.Login("email@example.com", "password").Result;

            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Login_ShouldGetAConsultantFromTheConsultantEndpoint()
        {
            var mockHttpHandler = new Mock<FakeHttpHandler>() { CallBase = true };

            _mockTokenSource.Setup(service => service.Fetch()).Returns(new OAuthToken());

            var apiClient = new ApiClient<MatchGuideApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var sut = new MatchGuideApi(apiClient);

            var result = sut.GetConsultant(1).Result;

            Assert.IsInstanceOf<Consultant>(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}
