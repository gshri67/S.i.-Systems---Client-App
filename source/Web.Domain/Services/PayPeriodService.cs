using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var timesheets = _timeSheetRepository.GetTimesheetsForUser(_sessionContext.CurrentUser.Id).ToList();
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
    }
}
