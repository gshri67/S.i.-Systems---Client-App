using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Consultants")]
    public class ConsultantsController: ApiController
    {
        private readonly ConsultantService _service;
        public ConsultantsController(ConsultantService service)
        {
            _service = service;
        }

        [Route("Alumni")]
        public HttpResponseMessage Get(string query)
        {
            var alumniContractorGroups = _service.FindAlumni(query);
            return Request.CreateResponse(HttpStatusCode.OK, alumniContractorGroups);
        }

        [Route("Alumni/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var consultant = _service.Find(id);
                return Request.CreateResponse(HttpStatusCode.OK, consultant);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ex);
            }
        }
    }
}
