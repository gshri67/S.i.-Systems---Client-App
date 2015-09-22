using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public string ProjectCode { get; set; }
		public string PayRate { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }

		public TimeEntry clone()
		{
			TimeEntry newEntry = new TimeEntry ();

			newEntry.Id = Id;
			newEntry.ProjectCode = ProjectCode;
			newEntry.PayRate = PayRate;
			newEntry.Hours = Hours;
			newEntry.Date = Date;

			return newEntry;
		}
    }
}
