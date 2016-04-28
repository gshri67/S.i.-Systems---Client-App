using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions_Sending : IContractCreationOptions_Sending
    {
        public List<InternalEmployee> ClientContactOptions { get; set; }
        public List<InternalEmployee> DirectReportOptions { get; set; }
        public List<InternalEmployee> BillingContactOptions { get; set; }

        public List<string> ClientContactNameOptions { get; set; }
        public List<string> DirectReportNameOptions { get; set; }
        public List<string> BillingContactNameOptions { get; set; }
        public List<string> ClientContractContactNameOptions { get; set; }
        public List<string> ReasonForNotSendingContractOptions { get; set; }

        public ContractCreationOptions_Sending()
        {
            ClientContactOptions = new List<InternalEmployee>();
            ClientContactNameOptions = new List<string>();
            DirectReportNameOptions = new List<string>();
            BillingContactNameOptions = new List<string>();
            ClientContractContactNameOptions = new List<string>();
            ReasonForNotSendingContractOptions = new List<string>();
        }
    }
}