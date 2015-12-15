using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ContractorDetailsTableViewModel
    {
        private Contractor _contractor;

        public ContractorDetailsTableViewModel( Contractor contractor )
        {
            _contractor = contractor;
        }

        public string FormattedPhoneNumberByRowNumber( int row )
        {
            if( _contractor.ContactInformation.PhoneNumbers.Count() > row )
                return _contractor.ContactInformation.PhoneNumbers.ElementAt(row).FormattedNumber;
            return string.Empty;
        }
        public string FormattedEmailByRowNumber(int row)
        {
            if (_contractor.ContactInformation.EmailAddresses.Count() > row)
                return _contractor.ContactInformation.EmailAddresses.ElementAt(row);
            return string.Empty;
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
    }
}