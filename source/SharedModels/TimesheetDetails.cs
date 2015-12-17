using System;

namespace SiSystems.SharedModels
{
	public class TimesheetDetails
	{
		public int Id { get; set;}
		public string ContractorFullName { get; set;}
		public string CompanyName { get; set;}
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public TimesheetDetails ()
		{
			ContractorFullName = string.Empty;
			CompanyName = string.Empty;
			StartDate = new DateTime ();
			EndDate = new DateTime ();
		}
	}
}

