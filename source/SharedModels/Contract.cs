using System;

namespace SiSystems.ClientApp.SharedModels
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

        public Decimal Rate { get; set; }

        public Contact Contact { get; set; }

        public int AgreementSubType { get; set; }
        public bool IsFloThru { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.FloThru; } }
        public bool IsFullService { get { return AgreementSubType == MatchGuideConstants.AgreementSubTypes.Consultant; } }
    }
}
