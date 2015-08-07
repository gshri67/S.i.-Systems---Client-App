using System;

namespace SiSystems.SharedModels
{
	public class Timesheet
	{
		public DateTime startDate;
		public DateTime endDate;
		public TimesheetStatus status;
		public string clientName;

		public string timePeriod
		{
			get
			{ 
					return startDate.ToString("MMM dd") + "-" + endDate.ToString("MMM dd");
			}
		}

		public enum TimesheetStatus
		{
			Open,
			Approved,
			Rejected,
			Pending
		};

		public Timesheet ()
		{
		}


	}
}

