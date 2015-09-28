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

        /// <summary>
        /// This is a constant value to correct for possible Floating Point issues
        /// </summary>
        private const float Tolerance = 0.00001F;

        public TimesheetService(ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository,
            ISessionContext sessionContext)
        {
            _timeSheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
            _sessionContext = sessionContext;
        }

        public Timesheet SaveTimesheet(Timesheet timesheet)
        {
            var userId = _sessionContext.CurrentUser.Id;

            var savedTimesheetId = _timeSheetRepository.SaveTimesheet(timesheet, userId);

            foreach (var entry in timesheet.TimeEntries)
                _timeEntryRepository.SaveTimeEntry(savedTimesheetId, entry);

            timesheet.OpenStatusId = savedTimesheetId;

            return timesheet;
        }

        public Timesheet GetTimesheetById(int id)
        {
            var updatedTimesheet = _timeSheetRepository.GetTimesheetsById(id);

            updatedTimesheet.TimeEntries = _timeEntryRepository.GetTimeEntriesByTimesheetId(id);

            return updatedTimesheet;
        }

        public Timesheet SubmitTimesheet(Timesheet timesheet)
        {
            timesheet = SubmittingZeroTimesheet(timesheet) 
                ? SubmitZeroTime(timesheet) 
                : SubmitTimesheetWithTimeEntries(timesheet);

            return GetTimesheetById(timesheet.Id);
        }

        private Timesheet SubmitTimesheetWithTimeEntries(Timesheet timesheet)
        {
            //timesheet.Id = _timeSheetRepository.
            return timesheet;
        }

        private Timesheet SubmitZeroTime(Timesheet timesheet)
        {
            timesheet.Id = _timeSheetRepository.SubmitZeroTimeForUser(timesheet, _sessionContext.CurrentUser.Id);
            
            return timesheet;
        }

        public static bool SubmittingZeroTimesheet(Timesheet timesheet)
        {
            if (timesheet.TimeEntries.Any())
            {
                return Math.Abs(timesheet.TimeEntries.Sum(entry => entry.Hours)) < Tolerance;
            }

            return true;
        }
    }
}
