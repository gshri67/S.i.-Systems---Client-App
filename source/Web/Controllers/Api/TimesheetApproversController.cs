using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorization]
    [RoutePrefix("api/TimesheetApprovers")]
    public class TimesheetApproversController: ApiController
    {
        private readonly TimesheetApproverService _service;
        public TimesheetApproversController(TimesheetApproverService service)
        {
            _service = service;
        }

        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var approvers = _service.GetTimesheetApproversByAgreementId(id);
            return Request.CreateResponse(HttpStatusCode.OK, approvers);
        }

        [Route("Request/{timesheetId}")]
        [HttpPost]
        public HttpResponseMessage RequestApprovalFromApproverWithId(int timesheetId)
        {
            var status = _service.RequestApprovalFromApproverWithId(timesheetId);
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }
    }
}
