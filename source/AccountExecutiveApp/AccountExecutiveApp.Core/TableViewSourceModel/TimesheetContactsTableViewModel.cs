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

	    public string ContractorFullName{ get { return _contact.Contractor.ContactInformation.FullName; } }
        public string DirectReportFullName { get { return _contact.DirectReport.FullName; } }
	    public int ContractorId { get { return _contact.Contractor.ContactInformation.Id; } }
        public int DirectReportId { get { return _contact.DirectReport.Id; } }

	    public TimesheetContactsTableViewModel(TimesheetContact contact )
		{
			_contact = contact;

			//_contractorTableModel = new ClientContactDetailsTableViewModel (_contact.Contractor.ContactInformation);
			//_contractorTableModel = new ClientContactDetailsTableViewModel (_contact.DirectReport);
		}


		public int NumberOfContractorPhoneNumbers()
		{
			return  _contact.Contractor.ContactInformation.PhoneNumbers.Count (); 
		}
		public int NumberOfDirectReportPhoneNumbers()
		{
			return  _contact.DirectReport.PhoneNumbers.Count (); 
		}

		public int NumberOfContractorEmails()
		{
			return  _contact.Contractor.ContactInformation.EmailAddresses.Count();
		}
		public int NumberOfDirectReportEmails()
		{
			return  _contact.DirectReport.EmailAddresses.Count();
		}

		public EmailAddress ContractorEmailAddressByRowNumber(int row)
		{
			if (_contact.Contractor.ContactInformation.EmailAddresses.Count() > row)
				return _contact.Contractor.ContactInformation.EmailAddresses.ElementAt(row);
			return new EmailAddress();
		}
		public EmailAddress DirectReportEmailAddressByRowNumber(int row)
		{
			if (_contact.DirectReport.EmailAddresses.Count() > row)
				return _contact.DirectReport.EmailAddresses.ElementAt(row);
			return new EmailAddress();
		}

		public PhoneNumber ContractorPhoneNumberByRowNumber(int row)
		{
			if (_contact.Contractor.ContactInformation.PhoneNumbers.Count() > row)
				return _contact.Contractor.ContactInformation.PhoneNumbers.ElementAt(row);
			return new PhoneNumber();
		}
		public PhoneNumber DirectReportPhoneNumberByRowNumber(int row)
		{
			if (_contact.DirectReport.PhoneNumbers.Count() > row)
				return _contact.DirectReport.PhoneNumbers.ElementAt(row);
			return new PhoneNumber();
		}

		/*
		public bool RowIsInBounds(int section, int rowNumber)
		{
			return section < NumberOfGroups() && rowNumber >= 0 && rowNumber < TimesheetByYear[section].Count();
		}

		public int NumberOfGroups()
		{
			return TimesheetByYear.Keys.Count;
		}

		public string CompanyNameBySectionAndRow(int section, int rowNumber )
		{
			return RowIsInBounds(section, rowNumber) 
				? TimesheetByYear[section].ElementAt(rowNumber).CompanyName
					: string.Empty;
		}

		public string ContractorFullNameBySectionAndRow(int section, int rowNumber)
		{
			return RowIsInBounds(section, rowNumber)
				? TimesheetByYear[section].ElementAt(rowNumber).ContractorFullName
					: string.Empty;
		}

		public string FormattedPeriodBySectionAndRow(int section, int rowNumber)
		{
			return RowIsInBounds(section, rowNumber)
				? string.Format("{0}-{1}", TimesheetByYear[section].ElementAt(rowNumber).StartDate.ToString("MMM d"),
					TimesheetByYear[section].ElementAt(rowNumber).EndDate.ToString("d")) 
					: string.Empty;
		}

		public int NumberOfTimesheetsInSection(int section)
		{
			return RowIsInBounds(section, 0)
				? TimesheetByYear[section].Count()
					: 0;
		}*/
	}
}
