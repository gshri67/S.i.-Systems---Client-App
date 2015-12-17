using System;

namespace SiSystems.SharedModels
{
	public class TimesheetSummarySet
	{
		public int Id { get; set; }
		public int NumOpen { get; set; }
		public int NumSubmitted { get; set; }
		public int NumRejected { get; set; }
		public int NumCancelled { get; set; }
	}
}

