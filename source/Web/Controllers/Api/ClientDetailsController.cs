using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Auth;
using SiSystems.ClientApp.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class ClientDetailsController: ApiController
    {
        private readonly ClientDetailsService _service;
        private readonly ISessionContext _session;

        public ClientDetailsController(ClientDetailsService service, ISessionContext session)
        {
            _service = service;
            _session = session;
        }

        [Authorize]
        public HttpResponseMessage Get()
        {
            var currentUser = (ApplicationUser)User.Identity;
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetClientDetails(_session.CurrentUser.ClientId));
        }
    }
}
