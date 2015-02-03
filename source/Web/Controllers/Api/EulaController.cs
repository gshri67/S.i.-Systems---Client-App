using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class EulaController : ApiController
    {
        private readonly EulaService _service;
        public EulaController(EulaService service)
        {
            _service = service;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetMostRecentEula());
        }
    }
}
