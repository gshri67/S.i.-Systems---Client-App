using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
	public class Timesheet
	{
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int ContractId { get; set; }
        public MatchGuideConstants.TimesheetStatus Status { get; set; }
		public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimesheetApprover { get; set; }

        //todo: should time period properties be contained within a seperate object?
		public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }
        public int AvailableTimePeriodId { get; set; }

        public int? OpenStatusId { get; set; }

	    public IEnumerable<TimeEntry> TimeEntries { get; set; }

        public int VerticalId { get { return 4; } }
	}

    public class PayPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

        public IEnumerable<Timesheet> Timesheets { get; set; }
    }
}

