using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsultantApp.Core
{
    public class TimeEntry
    {
        public String clientName, projectCode;
        public int hours;
        
        public TimeEntry() 
        {
        }
        
        public TimeEntry( String clientName, String projectCode, int hours )
        {
            this.clientName = clientName;
            this.projectCode = projectCode;
            this.hours = hours;
        }
    }
}
