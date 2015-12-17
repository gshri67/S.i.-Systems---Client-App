using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class TimesheetListViewModel
	{
		private readonly IMatchGuideApi _api;

		private TimesheetDetails _timesheet;

		public TimesheetDetails Timesheet
		{
			get { return _timesheet ?? new TimesheetDetails(); }
			private set { _timesheet = value ?? new TimesheetDetails(); }
		}

		public TimesheetListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

		public string CompanyName{ get{ return Timesheet.CompanyName; } }

		public string ContractorFullName{ get{ return Timesheet.ContractorFullName; } }

		public string FormattedStartDate{ get{ return Timesheet.StartDate.ToString ("MMM d"); } }
		public string FormattedEndDate{ get{ return Timesheet.StartDate.ToString ("MMM d"); } }
		public string FormattedPeriod{ get{ return string.Format("{0} - {1}", FormattedStartDate, FormattedEndDate); } }

		/*
		public Task LoadTimesheetDetails()
		{
			var task = GetTimesheetDetails();

			return task;
		}

		private async Task GetTimesheetDetails()
		{
			Timesheet = await _api.GetTimesheetDetails();
		}*/
	}
}
