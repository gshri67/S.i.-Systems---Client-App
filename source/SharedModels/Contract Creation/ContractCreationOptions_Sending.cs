using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions_Sending : IContractCreationOptions_Sending
    {
        public List<string> ClientContactNameOptions { get; set; }
        public List<string> DirectReportNameOptions { get; set; }
        public List<string> BillingContactNameOptions { get; set; }
        public List<string> ClientContractContactNameOptions { get; set; }
        public List<string> ReasonForNotSendingContractOptions { get; set; }
    }
}