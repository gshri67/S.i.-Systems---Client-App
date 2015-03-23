using ClientApp.Core.HttpAttributes;
using ClientApp.Core.Platform;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        class MockApi : ApiClient
        {
            public MockApi(ITokenStore tokenStore, IActivityManager activityManager, IErrorSource errorSource, IHttpMessageHandlerFactory handlerFactory)
                : base(tokenStore, activityManager, errorSource, handlerFactory)
            {
            }

            [HttpPost("failedlogin")]
            public async Task FailedLogin()
            {
                await this.ExecuteWithDefaultClient();
            }

            [HttpGet("mytesttype")]
            public async Task<MyTestType> GetMyTestType()
            {
                return await this.ExecuteWithDefaultClient<MyTestType>();
            }

            [HttpPost("mytest")]
            public async Task<MyTestType> PostMyTestType(MyTestType myTestType)
            {
                return await this.ExecuteWithDefaultClient<MyTestType>(myTestType);
            }

            [HttpPost("mystringcontenttest")]
            public async Task PostMyTestTypeWithStringData(string data)
            {
                await this.ExecuteWithDefaultClient(data);
            }

            [HttpGet("taskcancel")]
            public async Task<MyTestType> TaskCancel()
            {
                return await this.ExecuteWithDefaultClient<MyTestType>();
            }
        }

        class FakeHttpHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                HttpResponseMessage response = null;
                switch (request.RequestUri.AbsolutePath)
                {
                    case "/api/failedlogin":
                        const string errorResponseJson = "{\"error\":\"invalid_grant\",\"error_description\":\"The user name or password is incorrect.\"}";
                        response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { Content = new StringContent(errorResponseJson) };
                        break;
                    case "/api/mytesttype":
                        const string json = "{\"description\":\"My Test Type\"}";
                        response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(json) };
                        break;
                    case "/api/mytest":
                        response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = request.Content };
                        break;
                    case "/api/taskcancel":
                        Task.Delay(TimeSpan.FromMilliseconds(20)).Wait();
                        cancellationToken.ThrowIfCancellationRequested();
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
            _mockTokenSource = new Mock<ITokenStore>();
            _mockActivityManager = new Mock<IActivityManager>();
            _mockHttpHandlerHelper = new Mock<IHttpMessageHandlerFactory>();
            _mockErrorSource = new Mock<IErrorSource>();
            _mockHttpHandlerHelper.Setup(m => m.Get()).Returns(new FakeHttpHandler());
        }

        [Test]
        public void ApiClient_ShouldGetBaseAddressFromType()
        {
            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            Assert.AreEqual(BaseUrl, _sut.BaseAddress.AbsoluteUri);
        }

        [Test]
        public async void Authorize_ShouldStartAndStopActivity_WhenFailure()
        {
            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.FailedLogin();

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Get_ShouldStartAndStopActivity_WhenSuccess()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            var result = await _sut.GetMyTestType();

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Post_ShouldStartAndStopActivity_WhenSuccess()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.PostMyTestType(new MyTestType());

            _mockActivityManager.Verify(service => service.StartActivity(), Times.Once);
            _mockActivityManager.Verify(service => service.StopActivity(), Times.Once);
        }

        [Test]
        public async void Post_ShouldSendData_String()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.PostMyTestTypeWithStringData("my_data");
        }

        [Test]
        public async void Post_ShouldSerializeToJson_Obect()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            await _sut.PostMyTestType(new MyTestType { Description = "My Description" });
        }

        [Test]
        public async void Execute_ShouldBroadcastTimeoutError_OnTaskCancelled()
        {
            _mockTokenSource.Setup(service => service.GetDeviceToken()).Returns(new OAuthToken { Username = "email@example.com" });

            var _sut = new MockApi(_mockTokenSource.Object, _mockActivityManager.Object, _mockErrorSource.Object, _mockHttpHandlerHelper.Object);
            _sut.Timeout = TimeSpan.FromMilliseconds(1);
            var result = await _sut.TaskCancel();

            Assert.IsNull(result);
            _mockErrorSource.Verify(es => es.ReportError("Timeout", It.IsAny<string>(), true));
        }
    }
}
