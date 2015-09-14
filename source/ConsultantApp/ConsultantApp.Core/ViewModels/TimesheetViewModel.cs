using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class TimesheetViewModel
	{
        private readonly IMatchGuideApi _api;

		public static Dictionary<string, int> projectCodeDict;
		public static Dictionary<string, int> approverDict;//approvers are more likely to not be unique. handle this?

	    public TimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;

			if (projectCodeDict == null)
				projectCodeDict = new Dictionary<string, int> ();
	    }

		public void saveTimesheet(Timesheet timesheet)
		{
		}

		public Task<IEnumerable<string>> GetProjectCodes()
		{
			return _api.GetProjectCodes();
		}

		public Task<IEnumerable<string>> GetPayRates()
		{
			return _api.GetPayRates();
		}

        public Task<IEnumerable<string>> GetTimesheetApproversByTimesheetId(int timesheetId)
        {
            return _api.GetTimesheetApproversByTimesheetId(timesheetId);
        }
	}
}

