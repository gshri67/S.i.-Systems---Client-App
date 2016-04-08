using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions
    {
        //Details
        public IEnumerable<string> TimeFactorOptions { get; set; }
        public IEnumerable<string> DaysCancellationOptions { get; set; }
        public IEnumerable<string> LimitationExpenseOptions { get; set; }
        public IEnumerable<string> LimitationOfContractTypeOptions { get; set; }
        public IEnumerable<string> PaymentPlanOptions { get; set; }
        public IEnumerable<InternalEmployee> Colleagues { get; set; }
        public IEnumerable<string> InvoiceFrequencyOptions { get; set; }
        public IEnumerable<string> InvoiceFormatOptions { get; set; }

        //Rates
        public IEnumerable<string> RateTypeOptions { get; set; }
        public IEnumerable<string> RateDescriptionOptions { get; set; }

        //Sending
        public IEnumerable<string> ClientContactNameOptions { get; set; }
        public IEnumerable<string> DirectReportNameOptions { get; set; }
        public IEnumerable<string> BillingContactNameOptions { get; set; }
        public IEnumerable<string> ClientContractContactNameOptions { get; set; }
        public IEnumerable<string> ReasonForNotSendingContractOptions { get; set; }

        public ContractCreationOptions()
        {
            TimeFactorOptions = Enumerable.Empty<string>();
            DaysCancellationOptions = Enumerable.Empty<string>();
            LimitationExpenseOptions = Enumerable.Empty<string>();
            LimitationOfContractTypeOptions = Enumerable.Empty<string>();
            PaymentPlanOptions = Enumerable.Empty<string>();
            Colleagues = Enumerable.Empty<InternalEmployee>();
            InvoiceFrequencyOptions = Enumerable.Empty<string>();
            InvoiceFormatOptions = Enumerable.Empty<string>();

            RateTypeOptions = Enumerable.Empty<string>();
            RateDescriptionOptions = Enumerable.Empty<string>();

            ClientContactNameOptions = Enumerable.Empty<string>();
            DirectReportNameOptions = Enumerable.Empty<string>();
            BillingContactNameOptions = Enumerable.Empty<string>();
            ClientContractContactNameOptions = Enumerable.Empty<string>();
            ReasonForNotSendingContractOptions = Enumerable.Empty<string>();
        }
    }
}