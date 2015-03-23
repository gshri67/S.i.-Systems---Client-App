using ModernHttpClient;
using System.Net.Http;

namespace ClientApp.Core.Platform
{
    public class HttpMessageHandlerFactory
    {
        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new NativeMessageHandler();
        }
    }
}
