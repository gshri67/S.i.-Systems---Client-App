using System;

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
        public string RateType { get; set; }
        public string RateDescription { get; set; }
        public string BillRate { get; set; }
        public string PayRate { get; set; }
        public string ProposedRates { get; set; }
        public string GrossMargin { get; set; }
        public bool IsPrimaryRate { get; set; }

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

            RateType = string.Empty;
            RateDescription = string.Empty;
            BillRate = "0.00";
            PayRate = string.Empty;
            ProposedRates = string.Empty;
            GrossMargin = string.Empty;
            IsPrimaryRate = false;
        }
    }
}