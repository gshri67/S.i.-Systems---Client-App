using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
