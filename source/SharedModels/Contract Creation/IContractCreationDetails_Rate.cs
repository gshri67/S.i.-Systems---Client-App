using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public interface IContractCreationDetails_Rate
    {
        IEnumerable<string> RateTypes { get; set; }
        IEnumerable<string> RateDescriptions { get; set; }
        IEnumerable<string> BillRates { get; set; }
        IEnumerable<string> PayRates { get; set; }
        IEnumerable<string> ProposedRates { get; set; }
        IEnumerable<string> GrossMargins { get; set; }
        int PrimaryRateIndex { get; set; }
    }
}