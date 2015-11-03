using System;

namespace SiSystems.SharedModels
{
    public class ConsultantContract
    {
		public IM_Consultant consultant;

        public int ConsultantId { get; set; }

        public int ClientId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ClientContact DirectReport { get; set; }

        public bool IsFloThru;

        public bool IsFullySourced;

        //public MatchGuideConstants.ContractStatusTypes StatusType { get; set; }
        public MatchGuideConstants.ConsultantContractStatusTypes StatusType { get; set; }
    }
}
