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
	    public string TimesheetApprover;

		public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

	    public IEnumerable<TimeEntry> TimeEntries;
	}

    public class PayPeriod
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }
        
        public IEnumerable<Timesheet> Timesheets;
    }
}

