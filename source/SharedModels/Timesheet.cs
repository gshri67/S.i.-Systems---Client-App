using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public enum TimesheetStatus
	{
		Open,
		Approved,
		Rejected,
		Pending
	};

	public class Timesheet
	{
		public DateTime StartDate;
		public DateTime EndDate;
		public TimesheetStatus Status;
		public string ClientName;

		public string TimePeriod { get { return string.Format("{0:MMM dd}-{1:MMM dd}", StartDate, EndDate); } }

	    public IEnumerable<TimeEntry> TimeEntries;
	}
}

