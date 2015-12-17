using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/Timesheets")]
    public class TimesheetsController: ApiController
    {
        private readonly TimesheetService _service;
        public TimesheetsController(TimesheetService service)
        {
            _service = service;
        }

        public HttpResponseMessage Post(Timesheet timesheet)
        {
            var returnedTimesheet = _service.SaveTimesheet(timesheet);
            return Request.CreateResponse(HttpStatusCode.OK, returnedTimesheet);
        }

        [Route("Submit")]
        public HttpResponseMessage Submit(Timesheet timesheet)
        {
            var submittedTimesheet = _service.SubmitTimesheet(timesheet);
            return Request.CreateResponse(HttpStatusCode.OK, submittedTimesheet);
        }
    }

    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/Timesheets/Reporting")]
    public class TimesheetController : ApiController
    {
        private readonly TimesheetService _service;

        public TimesheetController(TimesheetService service)
        {
            _service = service;
        }

        [Route("Summary")]
        public HttpResponseMessage GetTimesheetSummary()
        {
            var timesheetsSummary = _service.GetTimesheetsSummary();
            return Request.CreateResponse(HttpStatusCode.OK, timesheetsSummary);
        }
    }
}
