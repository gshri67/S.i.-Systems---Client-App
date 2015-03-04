using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class ClientDetailsController: ApiController
    {
        private readonly ClientDetailsService _service;
        public ClientDetailsController(ClientDetailsService service)
        {
            _service = service;
        }

        [Authorize]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetAccountDetails());
        }
    }
}
