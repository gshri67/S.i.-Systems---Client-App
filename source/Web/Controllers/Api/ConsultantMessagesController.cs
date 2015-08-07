using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.SharedModels;

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
            _service.SendMessage(message);
            return Request.CreateResponse(HttpStatusCode.OK, "Message Sent");
        }
    }
}
