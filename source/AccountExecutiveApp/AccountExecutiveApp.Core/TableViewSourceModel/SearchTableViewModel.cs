using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class SearchTableViewModel
	{
        private IEnumerable<UserContact> _clientContacts;
        private IEnumerable<Contractor> _contractors;

        private IEnumerable<UserContact> _totalClientContacts;
        private IEnumerable<Contractor> _totalContractors;

        private IEnumerable<UserContact> _totalFilteredClientContacts;
        private IEnumerable<Contractor> _totalFilteredContractors;

	    private int _cellCap = 3;//how many maximum cells per section do we show on this screen?

        public SearchTableViewModel(IEnumerable<UserContact> clientContacts, IEnumerable<Contractor> contractors)
        {
            _clientContacts = clientContacts;
            _contractors = contractors;

            _totalClientContacts = clientContacts;
            _totalContractors = contractors;

            _totalFilteredClientContacts = clientContacts;
            _totalFilteredContractors = contractors;

            CapContacts();
        }

	    public void ApplyFilterWithText( string filter )
	    {
	        filter = filter.ToLower();

            _totalFilteredClientContacts = _totalClientContacts.Where(contact => contact.FullName.ToLower().Contains(filter)).OrderBy(x => x.FullName.ToLower().IndexOf(filter));
            _totalFilteredContractors = _totalContractors.Where(contractor => contractor.ContactInformation.FullName.ToLower().Contains(filter)).OrderBy(x => x.ContactInformation.FullName.ToLower().IndexOf(filter));

	        CapContacts();
	    }

	    private void CapContacts()
	    {
	        if (_totalFilteredClientContacts.Count() <= _cellCap)
	            _clientContacts = _totalFilteredClientContacts;
	        else
	            _clientContacts = _totalFilteredClientContacts.Take(_cellCap);

	        if (_totalFilteredContractors.Count() <= _cellCap)
	            _contractors = _totalFilteredContractors;
	        else
	            _contractors = _totalFilteredContractors.Take(_cellCap);
	    }

	    public int NumberOfClientContacts {get{ return _clientContacts.Count(); }}
        public int NumberOfContractors { get { return _contractors.Count(); } }

        public int NumberOfTotalFilteredContractors { get { return _totalFilteredContractors.Count(); } }
        public int NumberOfTotalFilteredClientContacts { get { return _totalFilteredClientContacts.Count(); } }

        public string ClientContactNameByRowNumber(int row)
        {
            if ( row >= 0 && row < _clientContacts.Count() ) 
                return _clientContacts.ElementAt(row).FullName;
            return string.Empty;
        }
        public string ClientCompanyNameByRowNumber(int row)
        {
            if (row >= 0 && row < _clientContacts.Count())
                return _clientContacts.ElementAt(row).ClientName;
            return string.Empty;
        }

        public string ContractorNameByRowNumber(int row)
        {
            if (row >= 0 && row < _contractors.Count())
                return _contractors.ElementAt(row).ContactInformation.FullName;
            return string.Empty;
        }

	    public int GetClientContactIdForIndex(int row)
	    {
	        if (row >= 0 && row < _clientContacts.Count())
	            return _clientContacts.ElementAt(row).Id;
	        return 0;
	    }
        public int GetContractorIdForIndex(int row)
        {
            if (row >= 0 && row < _contractors.Count())
                return _contractors.ElementAt(row).ContactInformation.Id;
            return 0;
        }

	    public UserContactType GetClientContactTypeForIndex(int row)
	    {
            return UserContactType.ClientContact;
	    }

	    public IEnumerable<UserContact> GetFilteredResultsForClientContacts()
	    {
	        return _totalFilteredClientContacts;
	    }
        public IEnumerable<UserContact> GetFilteredResultsForContractors()
        {
            return _totalFilteredContractors.Select( contractor => contractor.ContactInformation );
        }

	    public void ReloadWithFilteredContacts(IEnumerable<UserContact> filteredClientContacts, IEnumerable<Contractor> filteredContractors)
	    {
	        _totalFilteredClientContacts = filteredClientContacts;
	        _totalFilteredContractors = filteredContractors;

            CapContacts();
	    }
	}
}