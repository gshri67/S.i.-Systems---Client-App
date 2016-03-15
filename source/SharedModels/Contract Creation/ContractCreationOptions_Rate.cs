using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions_Rate : IContractCreationOptions_Rate
    {
        public List<string> RateTypeOptions { get; set; }
        public List<string> RateDescriptionOptions { get; set; }
    }
}