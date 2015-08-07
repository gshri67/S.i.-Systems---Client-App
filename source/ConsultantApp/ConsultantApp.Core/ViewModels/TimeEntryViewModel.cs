using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
    public class TimeEntryViewModel
    {
        //Local data: should be connected to back-end later
        public List<TimeEntry> timeEntries; //should be a list of lists or something.. 
        
        public TimeEntryViewModel() 
        {
            timeEntries = new List<TimeEntry>();
        }

        public List<TimeEntry> getTimeEntries( DateTime date ) 
        {
            List<TimeEntry> todayEntries = new List<TimeEntry>();

            if (timeEntries.Count > 0)
            {
                foreach ( TimeEntry entry in timeEntries )
                    if (entry.date.Equals(date))
                        todayEntries.Add( entry );

                return todayEntries;
            }

            if (todayEntries.Count > 0)
                return todayEntries;
            else
               return timeEntries;
        }

        /*
         * if the entry is already in our list, update it.
         * Otherwise add it to the list
         */
        public void saveTimeEntry(TimeEntry entry) 
        {
            //should we assume change by reference or add an ID?
            /*
            int index = 0;
            while( index < timeEntries.Count && timeEntries.ElementAt(index) != entry )
                index++;*/

            timeEntries.Add( entry );
        }

        public void saveTimeEntries( List<TimeEntry> entries ) 
        {
        }
    }
}
