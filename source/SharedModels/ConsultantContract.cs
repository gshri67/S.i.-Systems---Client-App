using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContractSummary
    {
        public int ContractId { get; set; }

        public string ContractorName { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public MatchGuideConstants.AgreementSubTypes AgreementSubType { get; set; }

        public bool IsFloThru { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.FloThru; } }

        public bool IsFullySourced { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.Consultant; } }

        public ContractStatusType StatusType { get; set; }
    }

    public class ConsultantContract
    {
        public int ContractId { get; set; }

		public int ConsultantId { get; set; }

        public int ContractId { get; set; }

		public string CompanyName { get; set; }
        
		public int ClientId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ClientContact DirectReport { get; set; }
        public ClientContact ClientContact { get; set; }
        public ClientContact BillingContact { get; set; }

        public ContractStatusType StatusType { get; set; }

        public MatchGuideConstants.AgreementSubTypes AgreementSubType { get; set; }

        public bool IsFloThru { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.FloThru; } }

        public bool IsFullySourced { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.Consultant; } }

        public float BillRate { get; set; }
        public float PayRate { get; set; }
        public float GrossMargin { get; set; }

        public IM_Consultant consultant;
    }

    public enum ContractStatusType
    {
        Ending, 
        Starting, 
        Active

        
    }
}
