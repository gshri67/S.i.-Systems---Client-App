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

		private TimesheetContact _contact;
		private int Id;

		public TimesheetContact Contact
		{
			get { return _contact ?? new TimesheetContact(); }
			private set { _contact = value ?? new TimesheetContact(); }
		}

		public TimesheetContactsViewModel(IMatchGuideApi api)
		{
			_api = api;
		}
		
		public Task LoadTimesheetContact( int newId )
		{
		    Id = newId;

			var task = GetTimesheetContactById( Id );

			return task;
		}

		private async Task GetTimesheetContactById( int Id )
		{
			Contact = await _api.GetTimesheetContactById( Id );
		}

	    public string CompanyName { get { return Contact.CompanyName; } }
        public string FormattedPeriod
        {
            get
            {
                return string.Format("{0}-{1} {2}", _contact.StartDate.ToString("MMM d"),
                    _contact.EndDate.ToString("dd").TrimStart('0'), _contact.StartDate.ToString("yyyy"));
            }
        }

       
	    public string PageTitle { get{ return string.Format("{0} Timesheet", _contact.Status.ToString() ); } }
        public string PageSubtitle { get { return string.Format("{0}, {1}", CompanyName, FormattedPeriod); } }
        
	}
}
