using System.Net.Http;

namespace Shared.Core.HttpAttributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute(string url) : base(HttpMethod.Post, url)
        {
        }
    }
}
