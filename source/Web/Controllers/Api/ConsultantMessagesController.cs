using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    public class ConsultantMessagesController : ApiController
    {
        private readonly ConsultantMessageService _service;
        public ConsultantMessagesController(ConsultantMessageService service)
        {
            _service = service;
        }

        public HttpResponseMessage Post(ConsultantMessage message)
        {
            _service.SendConsultantMessage(message);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
