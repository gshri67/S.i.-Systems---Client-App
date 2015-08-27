using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class TimesheetService
    {
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly ISessionContext _sessionContext;

        public TimesheetService(ITimesheetRepository timesheetRepository, ISessionContext sessionContext)
        {
            _timeSheetRepository = timesheetRepository;
            _sessionContext = sessionContext;
        }

        public Timesheet SaveTimesheet(Timesheet timesheet)
        {
            //do we want to do something with the user's Id?
            var userId = _sessionContext.CurrentUser.Id;

            var savedTimesheet = timesheet.Id == 0 ? 
                _timeSheetRepository.CreateTimesheet(timesheet) : 
                _timeSheetRepository.UpdateTimesheet(timesheet);
            return savedTimesheet;
        }
    }
}
