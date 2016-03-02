using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public float Hours { get; set; }
        public DateTime EntryDate { get; set; }
        public ProjectCodeRateDetails CodeRate { get; set; }
    }
}
