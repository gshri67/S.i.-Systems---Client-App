using System;
using System.Collections.Generic;
using ConsultantApp.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class TimesheetViewModel
	{
		public TimesheetViewModel ()
		{
		}

		public List<Timesheet> loadTimesheets()
		{
			List<Timesheet> timesheets = new List<Timesheet> ();

			for (int i = 0; i < 5; i++) 
			{
				Timesheet newTimesheet = new Timesheet ();
				newTimesheet.startDate = DateTime.Today.AddDays(-15);
				newTimesheet.endDate = DateTime.Today;
				newTimesheet.status = Timesheet.TimesheetStatus.Open;
				newTimesheet.clientName = "Cenovus";

				timesheets.Add( newTimesheet );
			}

			return timesheets;
		}
	}
}

