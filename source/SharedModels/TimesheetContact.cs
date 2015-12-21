using System;

namespace SiSystems.SharedModels
{
	public class TimesheetContact
	{
		public int Id;
		public Contractor Contractor { get; set; }
		public UserContact DirectReport { get; set;}

		public TimesheetContact ()
		{
		}
	}
}

