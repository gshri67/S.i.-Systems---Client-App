using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        //public HttpResponseMessage GetTimesheets()
        //{
        //    var timesheets = _service.GetOpenTimesheets(DateTime.Now);
        //    return Request.CreateResponse(HttpStatusCode.OK, timesheets);
        //}

        //[Route("Active")]
        //public HttpResponseMessage GetActiveTimesheets(DateTime date)
        //{
        //    var timesheets = _service.GetActiveTimesheets(date);
        //    return Request.CreateResponse(HttpStatusCode.OK, timesheets);
        //}

        //[Route("Entries")]
        //public HttpResponseMessage GetTimeEntries(DateTime date)
        //{
        //    var timesheets= _service.GetTimeEntries(date);
        //    return Request.CreateResponse(HttpStatusCode.OK, timesheets);
        //}
    }
}
