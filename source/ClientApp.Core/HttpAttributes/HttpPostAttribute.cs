using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core.HttpAttributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute(string url) : base(HttpMethod.Post, url)
        {
        }
    }
}
