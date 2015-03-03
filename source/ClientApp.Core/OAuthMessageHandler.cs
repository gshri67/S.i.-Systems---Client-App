using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class OAuthMessageHandler : DelegatingHandler
    {
        public OAuthToken Token { get; set; }

        public OAuthMessageHandler(HttpMessageHandler innerMessageHandler) : base(innerMessageHandler)
        {
            
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Token == null)
            {
                // throw something
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
