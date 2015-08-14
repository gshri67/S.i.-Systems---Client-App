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
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly ISessionContext _sessionContext;

        public PayPeriodService(ITimesheetRepository timesheetRepository, ISessionContext sessionContext)
        {
            _timeSheetRepository = timesheetRepository;
            _sessionContext = sessionContext;
        }

        public IEnumerable<PayPeriod> GetRecentPayPeriods()
        {
            var timesheets = _timeSheetRepository.GetTimesheetsForUser(_sessionContext.CurrentUser.Id).ToList();
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
