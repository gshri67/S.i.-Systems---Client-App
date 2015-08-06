using System.Net.Http;
using ModernHttpClient;

namespace Shared.Core.Platform
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
