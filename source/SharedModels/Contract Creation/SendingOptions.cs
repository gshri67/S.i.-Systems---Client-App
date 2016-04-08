using System.Collections.Generic;

namespace SiSystems.SharedModels.Contract_Creation
{
    public class SendingOptions
    {
        public IEnumerable<string> ClientContactNameOptions { get; set; }
        public IEnumerable<string> DirectReportNameOptions { get; set; }
        public IEnumerable<string> BillingContactNameOptions { get; set; }
        public IEnumerable<string> ClientContractContactNameOptions { get; set; }
        public IEnumerable<string> ReasonForNotSendingContractOptions { get; set; }
    }
}