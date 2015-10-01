using System.Collections.Generic;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class TimesheetApproverService
    {
        private readonly ISessionContext _sessionContext;
        private readonly IDirectReportRepository _timeSheetApproverRepository;


        public TimesheetApproverService(ISessionContext sessionContext , IDirectReportRepository directReportRepository )
        {
            _sessionContext = sessionContext;
            _timeSheetApproverRepository = directReportRepository;
        }

        public IEnumerable<DirectReport> GetTimesheetApproversByTimesheetId( int timesheetId )
        {
            return _timeSheetApproverRepository.GetPossibleApproversByTimesheetId(timesheetId);
        }
    }
}
