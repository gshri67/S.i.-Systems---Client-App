using ClientApp.Core.HttpAttributes;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.Core.Tests
{
    [TestFixture]
    public class ApiClientTests
    {
        const string BaseUrl = "http://myapi.test/api/";
        const string LoginUrl = "login";

        private Mock<ITokenStore> _mockTokenSource;
        private Mock<IActivityManager> _mockActivityManager;

        class MyTestType
        {
            public string Description { get; set; }
        }

        [Api(BaseUrl)]
        interface IMockApi
        {
            [HttpPost(LoginUrl)]
            void Login();

            [HttpPost("failedlogin")]
            void FailedLogin();

            [HttpGet("mytesttype")]
            MyTestType GetMyTestType();
        }

        class FakeHttpHandler : DelegatingHandler
        {
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                switch (request.RequestUri.AbsolutePath)
                {
                    case "/api/login":
                        const string jsonToken = "{\"access_token\": \"V9QKCstvkJ3XNmhn3iNwPnMQe7ecMacJFlBwA5ncSMPSE_E08eynYAUBSsMo4EmCzBsivAO3H3aeKTDXAibW2IQQmxAbl2m8IQgS2RWq4gzIIxm9sZGdCgt8KhFuuhCYkmGJzx5aQPggimpLs1E8PXrY82vA9ht9GWbaoQwE54RLwKQ9cBMWLsWsEyxB7LVI5DkoHmlDYHkBwvEAbowk3bHIYxEb0A_cTcEuvUeuGjKq4VW82I6UApPApYj2WEy2w-a8X9ybTVuswZ_w0JeRUVqMApiL9MwIY17JQuOpmyXL_yAMWACE4AKhl5GG1XRwG9d94BMpawkFi37dKQ_dREgOXX6pHR9Ohlh_HSxzyS8ALmeLfyra9H-dHE552Be_NOxlVg\",\"token_type\": \"bearer\", \"expires_in\": 1209599,\"userName\": \"email@example.com\",\r\n    \".issued\": \"Tue, 03 Mar 2015 23:10:16 GMT\", \".expires\": \"Tue, 17 Mar 2015 23:10:16 GMT\"}";
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(jsonToken) };
                    case "/api/failedlogin":
                        const string errorResponseJson = "{\"error\":\"invalid_grant\",\"error_description\":\"The user name orpassword is incorrect.\"}";
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { Content = new StringContent(errorResponseJson) };
                    case "/api/mytesttype":
                        const string json = "{\"description\":\"My Test Type\"}";
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(json) };
                    default:
                        throw new NotImplementedException("There is no fake data for this request");
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
        public void ApiClient_ShouldGetBaseAddressFromType()
        {
            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            Assert.AreEqual(BaseUrl, sut.BaseAddress.AbsoluteUri);
        }

        [Test]
        public void Authorize_ShouldStoreTheAuthorizationToken_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.Store(It.Is<OAuthToken>(token => token.Username == username)));

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Authenticate(username, password, "Login").Result;

            _mockTokenSource.VerifyAll();
        }

        [Test]
        public void Authorize_ShouldNotStoreTheAuthorizationToken_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.Store(It.Is<OAuthToken>(token => token.Username == username)));

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Authenticate(username, password, "LoginFailure").Result;

            _mockTokenSource.Verify(service => service.Store(It.IsAny<OAuthToken>()), Times.Never);
        }

        [Test]
        public void Authorize_ShouldStartAndStopActivity_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Authenticate(username, password, "Login").Result;

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test]
        public void Authorize_ShouldStartAndStopActivity_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Authenticate(username, password, "FailedLogin").Result;

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test]
        public void Get_ShouldStartAndStopActivity_WhenSuccess()
        {
            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);
            _mockTokenSource.Setup(service => service.Fetch()).Returns(new OAuthToken { Username = "email@example.com" });

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Get<MyTestType>(null, "GetMyTestType");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test]
        public void Get_ShouldNotStartAndStopActivity_IfNoToken()
        {
            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);
            _mockTokenSource.Setup(service => service.Fetch()).Returns((OAuthToken)null);

            var sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, new FakeHttpHandler());

            var result = sut.Get<MyTestType>(null, "GetMyTestType");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Never);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Never);
        }
    }
}
