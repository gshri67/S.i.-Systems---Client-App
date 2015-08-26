using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
    public class TimeEntryViewModel
    {
        private readonly IMatchGuideApi _api;
        
        //Local data: should be connected to back-end later
        public List<TimeEntry> timeEntries; //should be a list of lists or something.. 
        
        public TimeEntryViewModel(IMatchGuideApi api)
        {
            _api = api;
            timeEntries = new List<TimeEntry>();
        }

        /*
         * if the entry is already in our list, update it.
         * Otherwise add it to the list
         */
		public void saveTimeEntry(TimeEntry entry) //Timesheet timesheet, 
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
