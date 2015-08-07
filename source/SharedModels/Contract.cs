using System;

namespace SiSystems.SharedModels
{
    public class Contract
    {
        public int ConsultantId { get; set; }

        public int ClientId { get; set; }

        public string SpecializationName { get; set; }

        public string SpecializationNameShort { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal Rate { get; set; }
        public bool RateWitheld { get; set; }

        public Contact Contact { get; set; }

        public Contact DirectReport { get; set; }

        public MatchGuideConstants.AgreementSubTypes AgreementSubType { get; set; }

        public bool IsFloThru { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.FloThru; } }

        public bool IsFullySourced { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.Consultant; } }

        public MatchGuideConstants.ContractStatusTypes StatusType { get; set; }
    }
}
