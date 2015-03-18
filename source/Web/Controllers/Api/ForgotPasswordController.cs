using SiSystems.ClientApp.Web.Domain.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class ForgotPasswordController : ApiController
    {
        private readonly AccountService _service;

        public ForgotPasswordController(AccountService service)
        {
            this._service = service;
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]string emailAddress)
        {
            var result = await this._service.ForgotPassword(emailAddress);
            if (result.ResponseCode > 0)
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.BadRequest, result);
        }
    }
}
