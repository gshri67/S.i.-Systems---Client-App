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
}