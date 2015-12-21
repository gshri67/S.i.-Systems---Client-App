using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class TimesheetContactsViewModel
	{
		private readonly IMatchGuideApi _api;

		private IEnumerable<TimesheetContact> _timesheets;
		private int Id;

		public IEnumerable<TimesheetContact> Timesheets
		{
			get { return _timesheets ?? Enumerable.Empty<TimesheetContact>(); }
			private set { _timesheets = value ?? Enumerable.Empty<TimesheetContact>(); }
		}

		public TimesheetContactsViewModel(IMatchGuideApi api)
		{
			_api = api;
		}
		/*
		public Task LoadTimesheetContacts( MatchGuideConstants.TimesheetStatus status )
		{
			Status = status;

			var task = GetTimesheetContacts();

			return task;
		}

		private async Task GetTimesheetContacts()
		{
			Timesheets = await _api.GetTimesheetContacts( Id );
		}
*/
		//public string PageTitle { get{ return string.Format("{0} Timesheets", Status.ToString ); } }


	}
}
