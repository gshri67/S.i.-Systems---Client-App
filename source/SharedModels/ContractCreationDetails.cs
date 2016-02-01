﻿using System;

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

        public ContractCreationDetails()
        {
            JobTitle = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            TimeFactor = string.Empty;
            DaysCancellation = 10;
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
        }
    }
}