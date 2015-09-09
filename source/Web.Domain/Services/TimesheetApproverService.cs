using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class TimesheetApproverService
    {
        private readonly ISessionContext _sessionContext;
        private readonly ITimesheetApproverRepository _timeSheetApproverRepository;


        public TimesheetApproverService(ISessionContext sessionContext , ITimesheetApproverRepository timesheetApproverRepository )
        {
            _sessionContext = sessionContext;
            _timeSheetApproverRepository = timesheetApproverRepository;
        }

        public IEnumerable<string> GetTimesheetApprovers( int clientID )
        {/*
            var timesheets = _timeSheetRepository.GetTimesheetsForUser(_sessionContext.CurrentUser.Id).ToList();
            foreach (var timesheet in timesheets)
            {
                timesheet.TimeEntries = _timeEntryRepository.GetTimeEntriesByTimesheetId(timesheet.Id);
            }

            return (from @group in timesheets.GroupBy(t => t.TimePeriod)
                let period = @group.Key
                select new PayPeriod
                {
                    Timesheets = timesheets.Where(t => t.TimePeriod.Equals(period)), 
                    StartDate = @group.First().StartDate, 
                    EndDate = @group.First().EndDate
                }).ToList();*/

            List<string> list = new List<string>();
            list.Add("bob.smith@email.com");
            list.Add("fred.flintstone@email.com");
            list.Add("joe.johnson@email.com");
            list.Add("jessica.li@email.com");

            return list;
        }
    }
}
