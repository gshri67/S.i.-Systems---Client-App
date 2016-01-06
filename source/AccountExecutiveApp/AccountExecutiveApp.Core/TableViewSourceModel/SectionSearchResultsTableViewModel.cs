using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class SectionSearchResultsTableViewModel
	{
        private IEnumerable<UserContact> _contacts;
      
        public SectionSearchResultsTableViewModel(IEnumerable<UserContact> contacts)
        {
            _contacts = contacts;
        }

	    public int NumberOfContacts {get{ return _contacts.Count(); }}

        public string ContactNameByRowNumber(int row)
        {
            if ( row >= 0 && row < _contacts.Count() ) 
                return _contacts.ElementAt(row).FullName;
            return string.Empty;
        }
      
	}
}