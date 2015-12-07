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
            if( _contractor.PhoneNumbers.Count() > row )
                return _contractor.PhoneNumbers.ElementAt(row);
            return string.Empty;
        }
        public string FormattedEmailByRowNumber(int row)
        {
            if (_contractor.EmailAddresses.Count() > row)
                return _contractor.EmailAddresses.ElementAt(row);
            return string.Empty;
        }

        public int NumberOfPhoneNumbers()
        {
			return _contractor.PhoneNumbers.Count();
        }

        public int NumberOfEmails()
        {
			return _contractor.EmailAddresses.Count();
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