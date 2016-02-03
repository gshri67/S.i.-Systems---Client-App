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
        public IEnumerable<UserContact> AccountExecutiveOptions { get; set; }
        public IEnumerable<UserContact> GMAssignedOptions { get; set; }
        public IEnumerable<UserContact> ComissionAssignedOptions { get; set; }
        public IEnumerable<string> InvoiceFrequencyOptions { get; set; }
        public IEnumerable<string> InvoiceFormatOptions { get; set; }

        public ContractCreationOptions()
        {

        }
    }
}