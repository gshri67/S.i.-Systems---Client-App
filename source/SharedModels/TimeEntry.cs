using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public String ClientName { get; set; }
        public string ProjectCode { get; set; }
		public string PayRate { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
