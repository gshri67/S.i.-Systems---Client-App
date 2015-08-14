using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class ActiveTimesheetViewModel
	{
        private readonly IMatchGuideApi _api;

        public ActiveTimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
	    }

	    public Task<IEnumerable<Timesheet>> GetActiveTimesheets(DateTime date)
        {
            return _api.GetTimesheets(date);
        }

        public Task<IEnumerable<Timesheet>> GetCurrentActiveTimesheets()
        {
            return _api.GetTimesheets(DateTime.UtcNow);
        }

        public Task<IEnumerable<PayPeriod>> GetPayPeriods()
        {
            return _api.GetPayPeriods();
        }
	}
}

