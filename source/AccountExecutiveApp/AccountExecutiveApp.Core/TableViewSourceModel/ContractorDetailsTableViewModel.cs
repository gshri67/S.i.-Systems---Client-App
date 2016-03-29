using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ContractorDetailsTableViewModel
    {
        public Contractor Contractor;

        public ContractorDetailsTableViewModel( Contractor contractor )
        {
            Contractor = contractor;
        }

        public int NumberOfPhoneNumbers()
        {
            return Contractor.ContactInformation.PhoneNumbers.Count();
        }

        public int NumberOfEmails()
        {
            return Contractor.ContactInformation.EmailAddresses.Count();
        }

        public int NumberOfContracts()
        {
            return Contractor.Contracts.Count();
        }

        public IEnumerable<Specialization> Specializations
        {
            get { return Contractor.Specializations; }
        }

        public IEnumerable<ConsultantContract> Contracts
        {
            get { return Contractor.Contracts; }
        }

		public string ContractorResume 
		{
			get{ return Contractor.ResumeText; }
		}

        public EmailAddress EmailAddressByRowNumber(int row)
        {
            if (Contractor.ContactInformation.EmailAddresses.Count() > row)
                return Contractor.ContactInformation.EmailAddresses.ElementAt(row);
            return new EmailAddress();
        }

        public PhoneNumber PhoneNumberByRowNumber(int row)
        {
            if (Contractor.ContactInformation.PhoneNumbers.Count() > row)
                return Contractor.ContactInformation.PhoneNumbers.ElementAt(row);
            return new PhoneNumber();
        }

        public string LinkedInString { get { return Contractor.ContactInformation.LinkedInUrl; } }
    }
}