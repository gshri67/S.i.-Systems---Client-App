using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class SimpleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new {Message= "Hello World"});
        }
    }
}
