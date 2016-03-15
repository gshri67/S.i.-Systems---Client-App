using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public interface IContractCreationOptions_Sending
    {
        List<string> ClientContactNameOptions { get; set; }
        List<string> DirectReportNameOptions { get; set; }
        List<string> BillingContactNameOptions { get; set; }
        List<string> ClientContractContactNameOptions { get; set; }
        List<string> ReasonForNotSendingContractOptions { get; set; } 
    }
}