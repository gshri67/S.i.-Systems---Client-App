using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails_Rate
    {
        public string RateType { get; set; }
        public string RateDescription { get; set; }
        public float BillRate { get; set; }
        public float PayRate { get; set; }
        public float ProposedRate { get; set; }
        public float GrossMargin { get; set; }
        public bool isPrimaryRate { get; set; }
    }
}