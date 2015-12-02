using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ContractorDetailsTableViewModel
    {
        private Consultant _consultant;

        public ContractorDetailsTableViewModel( Consultant consultant )
        {
            _consultant = consultant;
        }

        public int NumberOfPhoneNumbers()
        {
            return 2;
        }

        public int NumberOfEmails()
        {
            return 1;
        }

        public int NumberOfContracts()
        {
            return 3;
        }
    }
}