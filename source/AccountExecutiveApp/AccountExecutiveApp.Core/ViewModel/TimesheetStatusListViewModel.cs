using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class TimesheetStatusListViewModel
	{
		private readonly IMatchGuideApi _api;

		private TimesheetSummarySet _timesheet;

		public TimesheetSummarySet Timesheet
		{
			get { return _timesheet ?? new TimesheetSummarySet(); }
			private set { _timesheet = value ?? new TimesheetSummarySet(); }
		}

		public TimesheetStatusListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

		public string FormattedNumberOfOpenTimesheets
		{
			get
			{
				return Timesheet.NumOpen.ToString();
			}
		}
		public string FormattedNumberOfSubmittedTimesheets
		{
			get
			{
				return Timesheet.NumSubmitted.ToString();
			}
		}
		public string FormattedNumberOfCancelledTimesheets
		{
			get
			{
				return Timesheet.NumCancelled.ToString();
			}
		}
		public string FormattedNumberOfRejectedTimesheets
		{
			get
			{
				return Timesheet.NumRejected.ToString();
			}
		}

		public Task LoadTimesheetSummary()
		{
			var task = GetTimesheetSummary();

			return task;
		}

		private async Task GetTimesheetSummary()
		{
			Timesheet = null;
		}
	}
}
