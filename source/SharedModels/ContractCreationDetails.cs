using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails
    {
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeFactor { get; set; }
        public int DaysCancellation { get; set; }
        public string LimitationExpense { get; set; }
        public string LimitationOfContract { get; set; }
        public string PaymentPlan { get; set; }
        public UserContact AccountExecutive { get; set; }
        public UserContact GMAssigned { get; set; }
        public UserContact ComissionAssigned { get; set; }
        public string InvoiceFrequency { get; set; }
        public string InvoiceFormat { get; set; }
        public Boolean UsingProjectCode { get; set; }
        public Boolean UsingQuickPay { get; set; }

        //Pay Rate info
        public IEnumerable<string> RateTypes { get; set; }
        public IEnumerable<string> RateDescriptions { get; set; }
        public IEnumerable<string> BillRates { get; set; }
        public IEnumerable<string> PayRates { get; set; }
        public IEnumerable<string> ProposedRates { get; set; }
        public IEnumerable<string> GrossMargins { get; set; }
        public int PrimaryRateIndex { get; set; }

        //Sending Info
        public bool IsSendingConsultantContract { get; set; }
        public string ClientContactName { get; set; }
        public string DirectReportName { get; set; }
        public string BillingContactName { get; set; }
        public string InvoiceRecipients { get; set; }
        public string ClientContractContact { get; set; }
        public bool IsSendingClientContract { get; set; }
        public string ReasonForNotSendingContract { get; set; }

        public ContractCreationDetails()
        {
            JobTitle = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            TimeFactor = string.Empty;
            DaysCancellation = 2;
            LimitationExpense = string.Empty;
            LimitationOfContract = string.Empty;
            PaymentPlan = string.Empty;
            AccountExecutive = new UserContact();
            GMAssigned = new UserContact();
            ComissionAssigned = new UserContact();
            InvoiceFrequency = string.Empty;
            InvoiceFormat = string.Empty;
            UsingProjectCode = false;
            UsingQuickPay = true;

            RateTypes = Enumerable.Empty<string>();
            RateDescriptions = Enumerable.Empty<string>();
            BillRates = Enumerable.Empty<string>();
            PayRates = Enumerable.Empty<string>();
            ProposedRates = Enumerable.Empty<string>();
            GrossMargins = Enumerable.Empty<string>();
            PrimaryRateIndex = 0;
        }
    }
}