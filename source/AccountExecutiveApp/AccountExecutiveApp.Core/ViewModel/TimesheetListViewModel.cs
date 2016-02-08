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

		private IEnumerable<TimesheetDetails> _timesheets;
	    private MatchGuideConstants.TimesheetStatus _status;
        public MatchGuideConstants.TimesheetStatus Status {
            get { return _status;  } }

        public IEnumerable<TimesheetDetails> Timesheets
		{
			get { return _timesheets ?? Enumerable.Empty<TimesheetDetails>(); }
            private set { _timesheets = value ?? Enumerable.Empty<TimesheetDetails>(); }
		}

		public TimesheetListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

		public Task LoadTimesheetDetails( MatchGuideConstants.TimesheetStatus status )
		{
			_status = status;

			var task = GetTimesheetDetails();

			return task;
		}

		private async Task GetTimesheetDetails()
		{
			Timesheets = await _api.GetTimesheetDetails( _status );
		}

		public string PageTitle { get{ return string.Format("{0} Timesheets", _status.ToString() ); } }
	}
}
