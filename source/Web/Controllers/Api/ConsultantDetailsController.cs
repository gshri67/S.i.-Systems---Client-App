using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/ConsultantDetails")]
    public class ConsultantDetailsController: ApiController
    {
        private readonly ConsultantDetailsService _service;

        public ConsultantDetailsController(ConsultantDetailsService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetCurrentUserConsultantDetails()
        {
            var consultantDetails = _service.GetCurrentUserConsultantDetails();
            return Request.CreateResponse(HttpStatusCode.OK, consultantDetails);
        }
    }
}
