using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContract
    {
		public int ConsultantId { get; set; }

		public string CompanyName { get; set; }
        
		public int ClientId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ClientContact DirectReport { get; set; }
        public ClientContact ClientContact { get; set; }
        public ClientContact BillingContact { get; set; }

        public bool IsFloThru;

        public bool IsFullySourced;

        //public ContractType StatusType { get; set; } //based on MatchGuideConstants.ConsultantContractStatusTypes
        public ContractStatusType StatusType { get; set; } //based on MatchGuideConstants.ConsultantContractStatusTypes
        public ContractType ContractType { get; set; } 

       

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

    public enum ContractType
    {
        FullySourced,
        FloThru
    }
}
