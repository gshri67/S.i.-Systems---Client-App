using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class PayPeriodService
    {
        private readonly ISessionContext _sessionContext;
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IDirectReportRepository _timeSheetApproverRepository;

        public PayPeriodService(ISessionContext sessionContext, ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository, IDirectReportRepository timeSheetApproverRepository)
        {
            _sessionContext = sessionContext;
            _timeSheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
            _timeSheetApproverRepository = timeSheetApproverRepository;
        }

        private static bool TimesheetIsOpen(Timesheet timesheet)
        {
            return timesheet.Status == MatchGuideConstants.TimesheetStatus.Open;
        }

        private IEnumerable<TimeEntry> GetTimeEntriesForTimesheet(Timesheet timesheet)
        {
            return TimesheetIsOpen(timesheet) 
                ? _timeEntryRepository.GetEntriesForOpenTimesheet(timesheet)
                : _timeEntryRepository.GetEntriesForNonOpenTimesheet(timesheet);
        }

        public IEnumerable<PayPeriod> GetRecentPayPeriods()
        {
            var timesheets = MostRecentSixMonthsOfTimesheets();

            foreach (var timesheet in timesheets)
            {
                timesheet.TimeEntries = GetTimeEntriesForTimesheet(timesheet).ToList();
            }

            return (from @group in timesheets.GroupBy(t => t.TimePeriod)
                let period = @group.Key
                select new PayPeriod
                {
                    Timesheets = timesheets.Where(t => t.TimePeriod.Equals(period)), 
                    StartDate = @group.First().StartDate, 
                    EndDate = @group.First().EndDate
                }).ToList();
        }

        private List<Timesheet> MostRecentSixMonthsOfTimesheets()
        {
            var nonOpenTimesheets = RemoveOlderTimesheets(_timeSheetRepository.GetNonOpenTimesheetsForUser(_sessionContext.CurrentUser.Id)).ToList();
            var openTimesheets = RemoveOlderTimesheets(_timeSheetRepository.GetOpenTimesheetsForUser(_sessionContext.CurrentUser.Id)).ToList();
            
            foreach (var timesheet in nonOpenTimesheets)
            {
                timesheet.TimesheetApprover =
                    _timeSheetApproverRepository.GetCurrentTimesheetApproverForTimesheet(timesheet.Id);
            }

            openTimesheets.AddRange(nonOpenTimesheets);

            return openTimesheets;
        }

        private static IEnumerable<Timesheet> RemoveOlderTimesheets(IEnumerable<Timesheet> timesheets)
        {
            return timesheets.Where(timesheet => timesheet.EndDate > DateTime.UtcNow.AddMonths(-6));
        } 
    }
}
