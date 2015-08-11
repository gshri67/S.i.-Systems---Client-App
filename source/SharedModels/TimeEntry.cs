using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public String ClientName { get; set; }
        public string ProjectCode { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
