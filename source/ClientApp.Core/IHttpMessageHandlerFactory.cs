using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
