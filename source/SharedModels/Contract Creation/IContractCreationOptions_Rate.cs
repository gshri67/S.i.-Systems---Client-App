using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public interface IContractCreationOptions_Rate
    {
        List<string> RateTypeOptions { get; set; }
        List<string> RateDescriptionOptions { get; set; } 
    }
}