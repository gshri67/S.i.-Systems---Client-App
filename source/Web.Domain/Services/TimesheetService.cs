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
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ISessionContext _sessionContext;

        public TimesheetService(ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository, ISessionContext sessionContext)
        {
            _timeSheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
            _sessionContext = sessionContext;
        }

        public Timesheet SaveTimesheet(Timesheet timesheet)
        {
            var userId = _sessionContext.CurrentUser.Id;

            var savedTimesheetId = _timeSheetRepository.SaveTimesheet(timesheet, userId);
            foreach ( var entry in timesheet.TimeEntries)
                _timeEntryRepository.SaveTimeEntry(savedTimesheetId, entry);

            timesheet.OpenStatusId = savedTimesheetId;

            return timesheet;
        }

        public Timesheet SubmitTimesheet(Timesheet timesheet)
        {
            var timesheetId = _timeSheetRepository.SubmitZeroTimeForUser(timesheet, _sessionContext.CurrentUser.Id);
            timesheet.Id = timesheetId;
            timesheet.Status = MatchGuideConstants.TimesheetStatus.Approved;
            
            return timesheet;
        }
    }
}
