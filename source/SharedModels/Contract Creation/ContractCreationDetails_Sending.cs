using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails_Sending
    {
        public bool IsSendingConsultantContract { get; set; }
        public string ClientContactName { get; set; }
        public string DirectReportName { get; set; }
        public string BillingContactName { get; set; }
        public string InvoiceRecipients { get; set; }
        public string ClientContractContactName { get; set; }
        public bool IsSendingClientContract { get; set; }
        public string ReasonForNotSendingContract { get; set; }
        public string SummaryReasonForNotSendingContract { get; set; }
    }
}