using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContract : ConsultantContractSummary
    {
        public int ConsultantId { get; set; }

		public int ClientId { get; set; }

        public UserContact DirectReport { get; set; }
        public UserContact ClientContact { get; set; }
        public UserContact BillingContact { get; set; }

        public float BillRate { get; set; }
        public float PayRate { get; set; }
        public float GrossMargin { get; set; }

        public Contractor Contractor { get; set; }
    }
}
