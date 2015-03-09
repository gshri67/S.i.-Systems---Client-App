using SiSystems.ClientApp.Web.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public async Task<HttpResponseMessage> Post([FromBody]string emailAddress)
        {
            var result = await this._service.ForgotPassword(emailAddress);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }
    }
}
