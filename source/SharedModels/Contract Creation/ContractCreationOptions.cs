using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions : IContractCreationOptions_Sending
    {
        //Details
        public IEnumerable<string> TimeFactorOptions { get; set; }
        public IEnumerable<int> DaysCancellationOptions { get; set; }
        public IEnumerable<string> LimitationExpenseOptions { get; set; }
        public IEnumerable<string> LimitationOfContractTypeOptions { get; set; }
        public IEnumerable<string> PaymentPlan { get; set; }
        public IEnumerable<InternalEmployee> Colleagues { get; set; }
        public IEnumerable<string> InvoiceFrequencyOptions { get; set; }
        public IEnumerable<string> InvoiceFormatOptions { get; set; }

        //Sending
        public List<string> ClientContactNameOptions { get; set; }
        public List<string> DirectReportNameOptions { get; set; }
        public List<string> BillingContactNameOptions { get; set; }
        public List<string> ClientContractContactNameOptions { get; set; }
        public List<string> ReasonForNotSendingContractOptions { get; set; }

        public ContractCreationOptions()
        {

        }
    }
}