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

		public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

	    public IEnumerable<TimeEntry> TimeEntries;

		public int totalHours( DateTime date )
		{
			int total = 0;

			foreach( TimeEntry entry in TimeEntries )
				if (entry.Date == date)
					total += entry.Hours;		

			return total;
		}
	}

    public class PayPeriod
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }
        
        public IEnumerable<Timesheet> Timesheets;
    }
}

