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

        public Task<IEnumerable<PayPeriod>> GetPayPeriods()
        {
            return _api.GetPayPeriods();
        }
	}
}

