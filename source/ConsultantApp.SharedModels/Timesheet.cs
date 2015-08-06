using System;

namespace ConsultantApp.SharedModels
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
				if (startDate != null && endDate != null)
					return startDate.ToString() + "-" + endDate.ToString();
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

