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
        private Mock<IHttpMessageHandlerFactory> _mockHttpHandlerHelper;
        private Mock<PassThroughExceptionHandler> _mockExceptionHandler;

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

            [HttpPost("mytest")]
            Task MyTest();
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
                    case "/api/mytest":
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
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
            _mockHttpHandlerHelper = new Mock<IHttpMessageHandlerFactory>();
            _mockHttpHandlerHelper.Setup(m => m.Get()).Returns(new FakeHttpHandler());
            _mockExceptionHandler = new Mock<PassThroughExceptionHandler>() { CallBase = true };
        }

        [Test]
        public void ApiClient_ShouldGetBaseAddressFromType()
        {
            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            Assert.AreEqual(BaseUrl, _sut.BaseAddress.AbsoluteUri);
        }

        [Test]
        public async void Authorize_ShouldStoreTheAuthorizationToken_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.SaveToken(It.Is<OAuthToken>(token => token.Username == username)));

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "Login");

            _mockTokenSource.VerifyAll();
        }

        [Test]
        public async void Authorize_ShouldNotStoreTheAuthorizationToken_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.SaveToken(It.Is<OAuthToken>(token => token.Username == username)));

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "LoginFailure");

            _mockTokenSource.Verify(service => service.SaveToken(It.IsAny<OAuthToken>()), Times.Never);
        }

        [Test]
        public async void Authorize_ShouldStartAndStopActivity_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "Login");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test]
        public async void Authorize_ShouldStartAndStopActivity_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "FailedLogin");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test]
        public async void Get_ShouldStartAndStopActivity_WhenSuccess()
        {
            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }

        [Test, ExpectedException(typeof(AuthorizationException))]
        public async void Get_ShouldThrow_IfNoToken()
        {
            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns((OAuthToken)null);

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");
        }

        [Test]
        public async void Post_ShouldStartAndStopActivity_WhenSuccess()
        {
            var activityGuid = new Guid("e8bf96ca-7a63-4b7c-970c-2a24e200ab68");
            _mockActivityManager.Setup(service => service.StartActivity(It.IsAny<CancellationToken>())).Returns(activityGuid);
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockExceptionHandler.Object, _mockHttpHandlerHelper.Object);
            await _sut.Post(null, "MyTest");

            _mockActivityManager.Verify(service => service.StartActivity(It.IsAny<CancellationToken>()), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(It.Is<Guid>(t => t == activityGuid)), Times.Once);
        }
    }
}
