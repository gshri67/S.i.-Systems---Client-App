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

        public IEnumerable<string> GetTimesheetApproversByTimesheetId( int timesheetId )
        {
            return _timeSheetApproverRepository.GetPossibleApproversByTimesheetId(timesheetId);
        }
    }
}
