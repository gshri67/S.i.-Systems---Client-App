using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class SearchTableViewModel
	{
        private IEnumerable<Contact> _clientContacts;
        private IEnumerable<Contractor> _contractors;

        public SearchTableViewModel(IEnumerable<Contact> clientContacts, IEnumerable<Contractor> contractors)
        {
            _clientContacts = clientContacts;
            _contractors = contractors;
        }

		public int NumberOfClientContacts {get{ return 5; }}
        public int NumberOfContractors { get { return 5; } }

        public string ClientContactNameByRowNumber(int row)
        {
            if ( row >= 0 && row < _clientContacts.Count() ) 
                return _clientContacts.ElementAt(row).FullName;
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