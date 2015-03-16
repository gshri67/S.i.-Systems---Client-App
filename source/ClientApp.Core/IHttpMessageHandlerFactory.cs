using ModernHttpClient;
using System.Net.Http;

namespace ClientApp.Core
{
    public interface IHttpMessageHandlerFactory
    {
        HttpMessageHandler Get();
    }

    public class NativeMessageHandlerFactory : IHttpMessageHandlerFactory
    {
        public HttpMessageHandler Get()
        {
            return new NativeMessageHandler();
        }
    }
}
