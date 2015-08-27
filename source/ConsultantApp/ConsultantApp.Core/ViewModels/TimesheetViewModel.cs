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

	    public TimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
	    }

	    public Task<IEnumerable<Timesheet>> GetTimesheets(DateTime date)
        {
            return _api.GetTimesheets(date);
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
	}
}

