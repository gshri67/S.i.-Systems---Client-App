using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class TimesheetContactsTableViewModel
	{
		private TimesheetContact _contact;

		//private ClientContactDetailsTableViewModel _contractorTableModel;
		//private ClientContactDetailsTableViewModel _directReportTableModel;

	    public string ContractorFullName{ get { return _contact.Contractor.FullName; } }
        public string DirectReportFullName { get { return _contact.DirectReport.FullName; } }
        public string ClientContactFullName { get { return _contact.ClientContact.FullName; } }

	    public int ContractorId { get { return _contact.Contractor.Id; } }
        public int DirectReportId { get { return _contact.DirectReport.Id; } }
        public int ClientContactId { get { return _contact.ClientContact.Id; } }

	    public TimesheetContactsTableViewModel(TimesheetContact contact )
		{
			_contact = contact;

			//_contractorTableModel = new ClientContactDetailsTableViewModel (_contact.Contractor.ContactInformation);
			//_contractorTableModel = new ClientContactDetailsTableViewModel (_contact.DirectReport);
		}


		public int NumberOfContractorPhoneNumbers()
		{
			return  _contact.Contractor.PhoneNumbers.Count (); 
		}
		public int NumberOfDirectReportPhoneNumbers()
		{
			return  _contact.DirectReport.PhoneNumbers.Count (); 
		}
        public int NumberOfClientContactPhoneNumbers()
        {
            return _contact.ClientContact.PhoneNumbers.Count();
        }

		public int NumberOfContractorEmails()
		{
			return  _contact.Contractor.EmailAddresses.Count();
		}
		public int NumberOfDirectReportEmails()
		{
			return  _contact.DirectReport.EmailAddresses.Count();
		}
        public int NumberOfClientContactEmails()
        {
            return _contact.ClientContact.EmailAddresses.Count();
        }

		public EmailAddress ContractorEmailAddressByRowNumber(int row)
		{
			if (_contact.Contractor.EmailAddresses.Count() > row)
				return _contact.Contractor.EmailAddresses.ElementAt(row);
			return new EmailAddress();
		}
		public EmailAddress DirectReportEmailAddressByRowNumber(int row)
		{
			if (_contact.DirectReport.EmailAddresses.Count() > row)
				return _contact.DirectReport.EmailAddresses.ElementAt(row);
			return new EmailAddress();
		}
        public EmailAddress ClientContactEmailAddressByRowNumber(int row)
        {
            if (_contact.ClientContact.EmailAddresses.Count() > row)
                return _contact.ClientContact.EmailAddresses.ElementAt(row);
            return new EmailAddress();
        }

		public PhoneNumber ContractorPhoneNumberByRowNumber(int row)
		{
			if (_contact.Contractor.PhoneNumbers.Count() > row)
				return _contact.Contractor.PhoneNumbers.ElementAt(row);
			return new PhoneNumber();
		}
		public PhoneNumber DirectReportPhoneNumberByRowNumber(int row)
		{
			if (_contact.DirectReport.PhoneNumbers.Count() > row)
				return _contact.DirectReport.PhoneNumbers.ElementAt(row);
			return new PhoneNumber();
		}
        public PhoneNumber ClientContactPhoneNumberByRowNumber(int row)
        {
            if (_contact.ClientContact.PhoneNumbers.Count() > row)
                return _contact.ClientContact.PhoneNumbers.ElementAt(row);
            return new PhoneNumber();
        }
	}
}
