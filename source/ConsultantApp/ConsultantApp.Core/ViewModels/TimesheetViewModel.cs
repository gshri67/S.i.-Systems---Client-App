using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using System.Collections;
using ConsultantApp.Core.ViewModels;

namespace ConsultantApp.Core.ViewModels
{
	public class TimesheetViewModel
	{
        private readonly IMatchGuideApi _api;

	    public TimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
	    }

		public Task SaveTimesheet(Timesheet timesheet)
		{
		    return _api.SaveTimesheet(timesheet);
		}

		public Task<IEnumerable<string>> GetProjectCodes()
		{
			return _api.GetProjectCodes();
		}

		public Task<IEnumerable<PayRate>> GetPayRates()
		{
			return _api.GetPayRates();
		}

        public Task<IEnumerable<string>> GetTimesheetApproversByTimesheetId(int timesheetId)
        {
            return _api.GetTimesheetApproversByTimesheetId(timesheetId);
        }
	}
}

