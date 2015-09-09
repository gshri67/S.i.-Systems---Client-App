using System;

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/TimesheetApprovers")]
    public class TimesheetApproversController: ApiController
    {
        private readonly TimesheetApproverService _service;
        public TimesheetApproversController(TimesheetApproverService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetTimesheetApprovers( int clientID )
        {
            var approvers = _service.GetTimesheetApprovers( clientID );
            return Request.CreateResponse(HttpStatusCode.OK, approvers);
        }
    }
}
