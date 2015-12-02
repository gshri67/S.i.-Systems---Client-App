using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractorDetailsViewModel
    {
        private Consultant _consultant;
        public Consultant Consultant
        {
            get { return _consultant ?? new Consultant(); }
            set { _consultant = value ?? new Consultant(); }
        }

        public string PageTitle {
            get { return "Bob Smith"; }
        }
    }
}