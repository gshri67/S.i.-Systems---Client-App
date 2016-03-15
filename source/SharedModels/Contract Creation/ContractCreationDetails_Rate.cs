using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails_Rate : IContractCreationDetails_Rate
    {
        public IEnumerable<string> RateTypes { get; set; }
        public IEnumerable<string> RateDescriptions { get; set; }
        public IEnumerable<string> BillRates { get; set; }
        public IEnumerable<string> PayRates { get; set; }
        public IEnumerable<string> ProposedRates { get; set; }
        public IEnumerable<string> GrossMargins { get; set; }
        public int PrimaryRateIndex { get; set; }
    }
}