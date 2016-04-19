using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails : IContractCreationDetails_Rate
    {
        public string ConsultantName { get; set; }

        //First Page
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeFactor { get; set; }
        public int DaysCancellation { get; set; }
        public string LimitationExpense { get; set; }
        public string LimitationOfContractType { get; set; }
        public int LimitationOfContractValue { get; set; }
        public string PaymentPlan { get; set; }
        public InternalEmployee AccountExecutive { get; set; }
        public InternalEmployee GMAssigned { get; set; }
        public InternalEmployee ComissionAssigned { get; set; }
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

        public IEnumerable<Rate> Rates { get; set; }

        //Sending Info
        public bool IsSendingConsultantContract { get; set; }
        public UserContact ClientContact { get; set; }
        public UserContact DirectReport { get; set; }
        public UserContact BillingContact { get; set; }
        public IEnumerable<UserContact> InvoiceRecipients { get; set; }
        public UserContact ClientContractContact { get; set; }
        public bool IsSendingClientContract { get; set; }
        public string ReasonForNotSendingContract { get; set; }
        public string SummaryReasonForNotSendingContract { get; set; }
        public string SummaryReasonForNotSendingConsultantContract { get; set; }
        
        public ContractCreationDetails()
        {
            ConsultantName = string.Empty;

            JobTitle = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            TimeFactor = string.Empty;
            DaysCancellation = 2;
            LimitationExpense = string.Empty;
            LimitationOfContractType = string.Empty;
            LimitationOfContractValue = 0;
            PaymentPlan = string.Empty;
            AccountExecutive = new InternalEmployee();
            GMAssigned = new InternalEmployee();
            ComissionAssigned = new InternalEmployee();
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

            Rates = Enumerable.Empty<Rate>();

            IsSendingConsultantContract = true;
            SummaryReasonForNotSendingConsultantContract = string.Empty;
            ClientContact = new UserContact();
            DirectReport = new UserContact();
            BillingContact = new UserContact();
            InvoiceRecipients = new List<UserContact>();
            ClientContractContact = new UserContact();
            IsSendingClientContract = true;
            ReasonForNotSendingContract = string.Empty;
            SummaryReasonForNotSendingContract = string.Empty;
        }
    }
}