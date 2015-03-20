using ClientApp.Core.HttpAttributes;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
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
        private Mock<IErrorSource> _mockErrorSource;

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

            [HttpPost("mystringcontenttest")]
            Task MyDataTest(string data);
        }

        class FakeHttpHandler : DelegatingHandler
        {
            public static HttpStatusCode StatusCode = HttpStatusCode.OK;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                HttpResponseMessage response;
                switch (request.RequestUri.AbsolutePath)
                {
                    case "/api/login":
                        const string jsonToken = "{\"access_token\": \"V9QKCstvkJ3XNmhn3iNwPnMQe7ecMacJFlBwA5ncSMPSE_E08eynYAUBSsMo4EmCzBsivAO3H3aeKTDXAibW2IQQmxAbl2m8IQgS2RWq4gzIIxm9sZGdCgt8KhFuuhCYkmGJzx5aQPggimpLs1E8PXrY82vA9ht9GWbaoQwE54RLwKQ9cBMWLsWsEyxB7LVI5DkoHmlDYHkBwvEAbowk3bHIYxEb0A_cTcEuvUeuGjKq4VW82I6UApPApYj2WEy2w-a8X9ybTVuswZ_w0JeRUVqMApiL9MwIY17JQuOpmyXL_yAMWACE4AKhl5GG1XRwG9d94BMpawkFi37dKQ_dREgOXX6pHR9Ohlh_HSxzyS8ALmeLfyra9H-dHE552Be_NOxlVg\",\"token_type\": \"bearer\", \"expires_in\": 1209599,\"userName\": \"email@example.com\",\r\n    \".issued\": \"Tue, 03 Mar 2015 23:10:16 GMT\", \".expires\": \"Tue, 17 Mar 2015 23:10:16 GMT\"}";
                        response = new HttpResponseMessage(StatusCode) { Content = new StringContent(jsonToken) };
                        break;
                    case "/api/failedlogin":
                        const string errorResponseJson = "{\"error\":\"invalid_grant\",\"error_description\":\"The user name or password is incorrect.\"}";
                        response = new HttpResponseMessage(StatusCode) { Content = new StringContent(errorResponseJson) };
                        break;
                    case "/api/mytesttype":
                        const string json = "{\"description\":\"My Test Type\"}";
                        response = new HttpResponseMessage(StatusCode) { Content = new StringContent(json) };
                        break;
                    case "/api/mytest":
                        response = new HttpResponseMessage(StatusCode);
                        break;
                    case "/api/mystringcontenttest":
                        var data = request.Content.ReadAsStringAsync().Result;
                        Assert.AreEqual("mydata", data);
                        response = new HttpResponseMessage(StatusCode) { Content = new StringContent(data) };
                        break;
                    default:
                        throw new NotImplementedException("There is no fake data for this request");
                }
                return Task.FromResult(response);
            }
        }

        [SetUp]
        public void SetUp()
        {
            FakeHttpHandler.StatusCode = HttpStatusCode.OK;
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
            _mockHttpHandlerHelper = new Mock<IHttpMessageHandlerFactory>();
            _mockErrorSource = new Mock<IErrorSource>();
            _mockHttpHandlerHelper.Setup(m => m.Get()).Returns(new FakeHttpHandler());
        }

        [Test]
        public void ApiClient_ShouldGetBaseAddressFromType()
        {
            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            Assert.AreEqual(BaseUrl, _sut.BaseAddress.AbsoluteUri);
        }

        [Test]
        public async void Authorize_ShouldStoreTheAuthorizationToken_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.SaveToken(It.Is<OAuthToken>(token => token.Username == username)));

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "Login");

            _mockTokenSource.VerifyAll();
        }

        [Test]
        public async void Authorize_ShouldStoreTheUsername_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.SaveUserName(It.Is<string>(user => user == username)));

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "Login");

            _mockTokenSource.VerifyAll();
        }

        [Test]
        public async void Authorize_ShouldNotStoreTheAuthorizationToken_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            _mockTokenSource.Setup(service => service.SaveToken(It.Is<OAuthToken>(token => token.Username == username)));
            FakeHttpHandler.StatusCode = HttpStatusCode.BadRequest;

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "FailedLogin");

            _mockTokenSource.Verify(service => service.SaveToken(It.IsAny<OAuthToken>()), Times.Never);
        }

        [Test]
        public async void Authorize_ShouldStartAndStopActivity_WhenSuccess()
        {
            const string username = "email@example.com";
            const string password = "password";

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "Login");

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Authorize_ShouldStartAndStopActivity_WhenFailure()
        {
            const string username = "email@example.com";
            const string password = "password";

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Authenticate(username, password, "FailedLogin");

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Get_ShouldStartAndStopActivity_WhenSuccess()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Get_ShouldDisplayAndBroadcastError_IfNoToken()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns((OAuthToken)null);

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockErrorSource.Verify(e => e.ReportError("TokenExpired", null, false), Times.Once);
        }

        [Test]
        public async void Post_ShouldStartAndStopActivity_WhenSuccess()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.Post(null, true, "MyTest");

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Post_ShouldSendData()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.Post(new StringContent("mydata"), true, "MyDataTest");
        }

        [Test]
        public async void Get_ShouldDeleteToken_HttpUnauthorized()
        {
            FakeHttpHandler.StatusCode = HttpStatusCode.Unauthorized;
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockErrorSource.Verify(e => e.ReportError("TokenExpired", null, true), Times.Once);
        }

        [Test]
        public async void Get_ShouldDeleteToken_HttpForbidden()
        {
            FakeHttpHandler.StatusCode = HttpStatusCode.Forbidden;
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockErrorSource.Verify(e => e.ReportError("TokenExpired", null, true), Times.Once);
        }

        [Test]
        public async void Get_ShouldDisplayError_OnServerError()
        {
            FakeHttpHandler.StatusCode = HttpStatusCode.InternalServerError;
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new ApiClient<IMockApi>(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.Get<MyTestType>(null, "GetMyTestType");

            _mockErrorSource.Verify(e => e.ReportError(null, null, false), Times.Once);
        }
    }
}
