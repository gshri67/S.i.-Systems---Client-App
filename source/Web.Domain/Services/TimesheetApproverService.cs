using System.Collections.Generic;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.Web.Domain.Context;

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

        public IEnumerable<string> GetTimesheetApproversByTimesheetId( int timesheetId )
        {
            return _timeSheetApproverRepository.GetPossibleApproversByTimesheetId(timesheetId);
        }
    }
}
