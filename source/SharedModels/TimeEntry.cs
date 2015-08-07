using System;

namespace SiSystems.SharedModels
{
    public class TimeEntry
    {
        public String clientName, projectCode;
        public int hours;
        public DateTime date;

        public TimeEntry()
        {
        }

        public TimeEntry(String clientName, String projectCode, int hours)
        {
            this.clientName = clientName;
            this.projectCode = projectCode;
            this.hours = hours;
        }
    }
}
