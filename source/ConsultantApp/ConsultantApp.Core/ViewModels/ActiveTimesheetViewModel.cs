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
		public static Dictionary<string, int> projectCodeDict;
		public static Dictionary<string, int> approverDict;

        public ActiveTimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;

			if (projectCodeDict == null)
				projectCodeDict = new Dictionary<string, int> ();
			if (approverDict == null)
				approverDict = new Dictionary<string, int> ();

			preloadDictionaries ();
		}

		//use the last few timesheets to populate most frequently used
		private async void preloadDictionaries()
		{
			IEnumerable<PayPeriod> payPeriods = await GetPayPeriods ();

			if (payPeriods != null) 
			{
				payPeriods.ToList ().Sort (new Comparison<PayPeriod> ((PayPeriod p1, PayPeriod p2) => {
					return p1.EndDate.CompareTo(p2.EndDate);
				}));

				int maxDepth = 3;//check the last # of periods to populate dictionaries
				if (payPeriods.Count () < maxDepth)
					maxDepth = payPeriods.Count ();

				for (int i = 0; i < maxDepth; i++) 
				{
					foreach (Timesheet timesheet in payPeriods.ElementAt(i).Timesheets ) 
					{
						foreach (TimeEntry entry in timesheet.TimeEntries) 
						{
							if (projectCodeDict.ContainsKey (entry.ProjectCode))
								projectCodeDict [entry.ProjectCode]++;
							else
								projectCodeDict.Add (entry.ProjectCode, 1);
						}

						if (approverDict.ContainsKey (timesheet.TimesheetApprover))
							approverDict [timesheet.TimesheetApprover]++;
						else
							approverDict.Add (timesheet.TimesheetApprover, 1);
					}
				}
			}
		}

        public Task<IEnumerable<PayPeriod>> GetPayPeriods()
        {
            return _api.GetPayPeriods();
        }

	    public Task<ConsultantDetails> GetConsultantDetails()
	    {
	        return _api.GetCurrentUserConsultantDetails();
	    }
	}
}

