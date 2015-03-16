using ModernHttpClient;
using System.Net.Http;

namespace ClientApp.Core
{
    public class HttpMessageHandlerFactory
    {
        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new NativeMessageHandler();
        }
    }
}
