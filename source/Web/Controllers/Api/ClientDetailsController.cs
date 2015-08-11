using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Auth;
using SiSystems.Web.Domain.Context;

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
        public IHttpActionResult Get()
        {
            var details = _service.GetClientDetails();
            return Ok(details);
        }
    }
}
