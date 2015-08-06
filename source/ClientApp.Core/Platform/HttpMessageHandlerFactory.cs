using System.Net.Http;
using ModernHttpClient;

namespace Shared.Core.Platform
{
    public class HttpMessageHandlerFactory
    {
        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new NativeMessageHandler();
        }
    }
}
