using System.Net.Http;

namespace ClientApp.Core.HttpAttributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute(string url) : base(HttpMethod.Post, url)
        {
        }
    }
}
