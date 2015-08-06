using System.Net.Http;

namespace Shared.Core.HttpAttributes
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute(string url) : base(HttpMethod.Get, url)
        {
        }
    }
}
