﻿using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class ContractProposal
    {
        public int ConsultantId { get; set; }
        public int ClientId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Decimal Rate { get; set; }

        public Decimal Fee { get; set; }
        
        public string TimesheetApproverEmailAddress { get; set; }
    }
}
