using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Specializations")]
    public class SpecializationsController : ApiController
    {
        private readonly SpecializationService _service;

        public SpecializationsController(SpecializationService service)
        {
            _service = service;
        }

        public async Task<HttpResponseMessage> Get(int id)
        {
            var specialization = await _service.GetAsync(id);
            return specialization != null
                ? Request.CreateResponse(HttpStatusCode.OK, specialization)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    new HttpError(string.Format("Specialization with id {0} not found.", id)));
        }

        public async Task<HttpResponseMessage> Get()
        {
            var specializations = await _service.GetAllAsync();
            return Request.CreateResponse(HttpStatusCode.OK, specializations);
        }
    }
}
