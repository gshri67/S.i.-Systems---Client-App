using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Timesheets")]
    public class TimesheetsController: ApiController
    {
        private readonly TimesheetService _service;
        public TimesheetsController(TimesheetService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetTimesheets()
        {
            var timesheets = _service.GetOpenTimesheets(DateTime.Now);
            return Request.CreateResponse(HttpStatusCode.OK, timesheets);
        }

        [Route("Entries")]
        public HttpResponseMessage GetTimeEntries(DateTime date)
        {
            var timesheets= _service.GetTimeEntries(date);
            return Request.CreateResponse(HttpStatusCode.OK, timesheets);
        }
    }
}
