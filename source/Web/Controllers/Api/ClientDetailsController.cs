using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    public class ClientDetailsController: ApiController
    {
        private readonly ClientDetailsService _service;
        public ClientDetailsController(ClientDetailsService service)
        {
            _service = service;
        }

        public HttpResponseMessage Get()
        {
            var clientDetails = _service.GetAccountDetails();
            return Request.CreateResponse(HttpStatusCode.OK, clientDetails);
        }
    }
}
