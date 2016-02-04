using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ContractCreationOptions
    {
        public IEnumerable<string> TimeFactorOptions { get; set; }
        public IEnumerable<int> DaysCancellationOptions { get; set; }
        public IEnumerable<string> LimitationExpenseOptions { get; set; }
        public IEnumerable<string> LimitationOfContractOptions { get; set; }
        public IEnumerable<string> PaymentPlan { get; set; }
        public IEnumerable<InternalEmployee> Colleagues { get; set; }
        public IEnumerable<string> InvoiceFrequencyOptions { get; set; }
        public IEnumerable<string> InvoiceFormatOptions { get; set; }

        public ContractCreationOptions()
        {

        }
    }
}