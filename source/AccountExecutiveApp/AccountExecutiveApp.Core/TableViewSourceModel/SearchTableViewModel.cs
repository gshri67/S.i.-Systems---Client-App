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

            _totalFilteredClientContacts = Enumerable.Empty<UserContact>();
            _totalFilteredContractors = Enumerable.Empty<Contractor>();
        }

	    public void ApplyFilterWithText( string filter )
	    {
	        _totalFilteredClientContacts = _totalClientContacts.Where(contact => contact.FirstName.StartsWith(filter));
            _totalFilteredContractors = _totalContractors.Where(contractor => contractor.ContactInformation.FirstName.StartsWith(filter));

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
	}
}

/*
namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class ContractorDetailsTableViewModel
	{
		private Contractor _contractor;

		public ContractorDetailsTableViewModel( Contractor contractor )
		{
			_contractor = contractor;
		}

		public int NumberOfPhoneNumbers()
		{
			return _contractor.ContactInformation.PhoneNumbers.Count();
		}

		public int NumberOfEmails()
		{
			return _contractor.ContactInformation.EmailAddresses.Count();
		}

		public int NumberOfContracts()
		{
			return _contractor.Contracts.Count();
		}

		public IEnumerable<Specialization> Specializations
		{
			get { return _contractor.Specializations; }
		}

		public IEnumerable<ConsultantContract> Contracts
		{
			get { return _contractor.Contracts; }
		}

		public string ContractorResume 
		{
			get{ return _contractor.ResumeText; }
		}

		public EmailAddress EmailAddressByRowNumber(int row)
		{
			if (_contractor.ContactInformation.EmailAddresses.Count() > row)
				return _contractor.ContactInformation.EmailAddresses.ElementAt(row);
			return new EmailAddress();
		}

		public PhoneNumber PhoneNumberByRowNumber(int row)
		{
			if (_contractor.ContactInformation.PhoneNumbers.Count() > row)
				return _contractor.ContactInformation.PhoneNumbers.ElementAt(row);
			return new PhoneNumber();
		}
	}
}*/