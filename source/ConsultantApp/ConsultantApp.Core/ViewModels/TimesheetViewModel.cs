using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
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

		public Task<Timesheet> SaveTimesheet(Timesheet timesheet)
		{
		    return _api.SaveTimesheet(timesheet);
		}

		public Task<IEnumerable<string>> GetProjectCodes()
		{
			return _api.GetProjectCodes();
		}

		public Task<IEnumerable<PayRate>> GetPayRates(int contractId)
		{
			return _api.GetPayRates(contractId);
		}

        public Task<IEnumerable<DirectReport>> GetTimesheetApproversByTimesheetId(int timesheetId)
        {
            return _api.GetTimesheetApproversByTimesheetId(timesheetId);
        }

	    public Task<Timesheet> SubmitTimesheet(Timesheet timesheet)
	    {
	        return _api.SubmitTimesheet(timesheet);
	    }

	    public Task<Timesheet> WithdrawTimesheet(Timesheet timesheet)
	    {
            //todo:Make the Withdraw Timesheet Call to the API
	        var apiCallResult = new TaskCompletionSource<Timesheet>();
            apiCallResult.SetResult(timesheet);
            return apiCallResult.Task;
	    }
	}
}

