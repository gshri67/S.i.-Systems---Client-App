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

	    private IEnumerable<PayPeriod> _payPeriods;
        public IEnumerable<PayPeriod> PayPeriods
	    {
	        get { return _payPeriods; }
            private set { _payPeriods = value ?? Enumerable.Empty<PayPeriod>(); }
	    }

        public ActiveTimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
            
            PayPeriods = Enumerable.Empty<PayPeriod>();
        }

	    public Task LoadPayPeriods()
	    {
            var loadingPayPeriods = GetPayPeriods();
	        return loadingPayPeriods;
	    }

        private async Task GetPayPeriods()
        {
#if TEST
            Console.WriteLine("GetPayPeriods");
#endif
            PayPeriods = await _api.GetPayPeriodSummaries();
        }
        
        public bool UserHasPayPeriods()
        {
            return PayPeriods != null && PayPeriods.Any();
        }
	}
}

