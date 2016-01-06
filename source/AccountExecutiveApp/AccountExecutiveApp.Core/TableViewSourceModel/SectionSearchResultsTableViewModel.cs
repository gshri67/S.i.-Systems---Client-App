using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class SectionSearchResultsTableViewModel
	{
        private IEnumerable<UserContact> _contacts;
	    public bool IsClientContact;

	    public SectionSearchResultsTableViewModel(IEnumerable<UserContact> contacts, bool isClientContact )
        {
            _contacts = contacts;
	        IsClientContact = isClientContact;
        }

	    public int NumberOfContacts {get{ return _contacts.Count(); }}

        public string ContactNameByRowNumber(int row)
        {
            if ( row >= 0 && row < _contacts.Count() ) 
                return _contacts.ElementAt(row).FullName;
            return string.Empty;
        }

	    public string ContactCompanyNameByRowNumber(int row)
        {
            if (row >= 0 && row < _contacts.Count())
                return _contacts.ElementAt(row).ClientName;
            return string.Empty;
        }

	    public int GetContactIdForIndex(int row)
	    {
            if (row >= 0 && row < _contacts.Count())
                return _contacts.ElementAt(row).Id;
            return 0;
	    }

	    public UserContactType GetClientContactTypeForIndex(int item)
	    {
	        return UserContactType.ClientContact;
	    }
	}
}