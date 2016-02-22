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

        public PayPeriodService(ISessionContext sessionContext, ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository)
        {
            _sessionContext = sessionContext;
            _timeSheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
        }

        public IEnumerable<PayPeriod> GetRecentPayPeriods()
        {
            var timesheets = MostRecentSixMonthsOfTimesheets();

            foreach (var timesheet in timesheets)
            {
                timesheet.TimeEntries = _timeEntryRepository.GetTimeEntriesByTimesheetId(timesheet.Id);
                foreach (var entry in timesheet.TimeEntries)
                {
                    entry.PayRate = _timeEntryRepository.GetPayRateById(entry);
                }
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
            var allTimesheets = _timeSheetRepository.GetNonOpenTimesheetsForUser(_sessionContext.CurrentUser.Id);

            var openTimesheets = _timeSheetRepository.GetOpenTimesheetsForUser(_sessionContext.CurrentUser.Id);

            var timesheets = openTimesheets.ToList();

            timesheets.AddRange(
                allTimesheets//.Where(timesheet => timesheet.Status != MatchGuideConstants.TimesheetStatus.Cancelled)
            );

            //only retrieve the most recent 6 months worth of timesheets
            timesheets = timesheets.Where(ts => ts.EndDate > DateTime.UtcNow.AddMonths(-6)).ToList();
            return timesheets;
        }
    }
}
