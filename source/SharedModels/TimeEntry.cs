using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public String ClientName { get; set; }
        public string ProjectCode { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
    }
}
