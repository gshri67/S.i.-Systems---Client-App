﻿using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContractSummary
    {
        public int ContractId { get; set; }

        public string ContractorName { get; set; }
        public string ClientName { get; set; }
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public MatchGuideConstants.AgreementSubTypes AgreementSubType { get; set; }

        public bool IsFloThru { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.FloThru; } }

        public bool IsFullySourced { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.Consultant; } }

        public ContractStatusType StatusType { get; set; }
    }

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
		public float Markup { get; set; }

        public Contractor Contractor { get; set; }
    }
}
