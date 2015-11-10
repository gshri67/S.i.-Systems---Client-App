using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContract
    {
		public int ConsultantId { get; set; }

        public int ClientId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ClientContact DirectReport { get; set; }

        public bool IsFloThru;

        public bool IsFullySourced;

        public string StatusType { get; set; } //based on MatchGuideConstants.ConsultantContractStatusTypes
        
        public IM_Consultant consultant;
    }
}
