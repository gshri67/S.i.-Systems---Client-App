
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/UserContact")]
    public class UserContactController : ApiController
    {
        private readonly UserContactService _service;

        public UserContactController(UserContactService service)
        {
            _service = service;
        }
        
        [Route("{id}")]
        public HttpResponseMessage GetUserContactById(int id)
        {
            var userContact = _service.GetUserContactById(id);
            return Request.CreateResponse(HttpStatusCode.OK, userContact);
        }

        public HttpResponseMessage GetClientContacts()
        {
            var userContacts = _service.GetClientContacts();
            return Request.CreateResponse(HttpStatusCode.OK, userContacts);
        }

        [Route("Filter/{filter}")]
        public HttpResponseMessage GetClientContactsWithFilter( string filter )
        {
            var userContacts = _service.GetClientContacts();
            return Request.CreateResponse(HttpStatusCode.OK, userContacts);
        }
    }
}
