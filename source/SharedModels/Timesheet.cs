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
        public int Id { get; set; }
		public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimesheetStatus Status { get; set; }
        public string ClientName { get; set; }
        public string TimesheetApprover { get; set; }

		public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

	    public IEnumerable<TimeEntry> TimeEntries { get; set; }
	}

    public class PayPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

        public IEnumerable<Timesheet> Timesheets { get; set; }
    }
}

