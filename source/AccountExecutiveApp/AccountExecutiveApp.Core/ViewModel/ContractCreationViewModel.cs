using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractCreationViewModel
    {
        private readonly IMatchGuideApi _api;

        private ContractCreationDetails _contract;
        public ContractCreationDetails Contract
        {
            get { return _contract ?? new ContractCreationDetails(); }
            set { _contract = value ?? new ContractCreationDetails(); }
        }

        public int JobId;
        public int CandidateId;

        public string ConsultantName
        {
            get { return Contract.ConsultantName; }
            set { Contract.ConsultantName = value; }
        }

        public ContractCreationViewModel(IMatchGuideApi api )
        {
            _api = api;
        }

        //First Page
        public DateTime StartDate {
                                                get { return Contract.StartDate; }
                                                set { Contract.StartDate = value; } }
        public DateTime EndDate
        {
            get { return Contract.EndDate; }
            set { Contract.EndDate = value; }
        }

        public string FormattedEndDate
        {
            get
            {
                //if (!_successfullyLoadedContracts)
                //  return string.Empty;

                return string.Format("{0}", Contract.EndDate.ToString("MMM dd, yyyy"));
            }
        }
        public string FormattedStartDate
        {
            get
            {
                //if (!_successfullyLoadedContracts)
                //  return string.Empty;

                return string.Format("{0}", Contract.StartDate.ToString("MMM dd, yyyy"));
            }
        }


        public string JobTitle {                get { return Contract.JobTitle; }
                                                set { Contract.JobTitle = value; }}

        public string TimeFactor {              get { return Contract.TimeFactor; }
                                                set { Contract.TimeFactor = value; }}
        public int DaysCancellation {           get { return Contract.DaysCancellation; }
                                                set { Contract.DaysCancellation = value; }}

        public string LimitationExpense {       get { return Contract.LimitationExpense; }
                                                set { Contract.LimitationExpense = value; }}
        public string LimitationOfContractType {    get { return Contract.LimitationOfContractType; } 
                                                set { Contract.LimitationOfContractType = value; } }
        public int LimitationOfContractValue {    get { return Contract.LimitationOfContractValue; } 
                                                set { Contract.LimitationOfContractValue = value; } }

        public string PaymentPlan {             get { return Contract.PaymentPlan; }
                                                set { Contract.PaymentPlan = value; }}
        public InternalEmployee AccountExecutive
        {
            get { return Contract.AccountExecutive; }
                                                set { Contract.AccountExecutive = value; }}

        public InternalEmployee GMAssigned
        {
            get { return Contract.GMAssigned; } 
                                                set { Contract.GMAssigned = value; } }

        public InternalEmployee ComissionAssigned
        {
            get { return Contract.ComissionAssigned; } 
                                                set { Contract.ComissionAssigned = value; } }

        public string InvoiceFrequency {        get { return Contract.InvoiceFrequency; } 
                                                set { Contract.InvoiceFrequency = value; } }
        public string InvoiceFormat {           get { return Contract.InvoiceFormat; } 
                                                set { Contract.InvoiceFormat = value; } }
        public Boolean UsingProjectCode {       get { return Contract.UsingProjectCode; }
                                                set { Contract.UsingProjectCode = value; }}
        public Boolean UsingQuickPay {          get { return Contract.UsingQuickPay; } 
                                                set { Contract.UsingQuickPay = value; } }



        //Pay Rates
        public string RateTypeAtIndex(int index){ return Contract.Rates.ElementAt(index).RateType; }
        public void SetRateTypeAtIndex(string newRateType, int index) 
        {
            //for now the rate types need to be the same for all rates anyways so just update them to be the same
            foreach (var rate in Contract.Rates)
                rate.RateType = newRateType;
        }
        public bool AreRateTypesPerDay{ get { if( Contract.Rates != null && Contract.Rates.Any() ) return Contract.Rates.ElementAt(0).RateType == "Per day";
            return false;
        }}

        public float HoursPerDayAtIndex(int index) { return Contract.Rates.ElementAt(index).HoursPerDay; }
        public void SetHoursPerDayAtIndex(int newHoursPerDay, int index)
        {
            Contract.Rates.ElementAt(index).HoursPerDay = newHoursPerDay;
        }

        public string RateDescriptionAtIndex(int index) { return Contract.Rates.ElementAt(index).Description; }
        public void SetRateDescriptionAtIndex(string newRateDescription, int index)
        {
            Contract.Rates.ElementAt(index).Description = newRateDescription;
        }

        public string BillRateAtIndex(int index) { return Contract.Rates.ElementAt(index).BillRate.ToString(); }
        public void SetBillRateAtIndex(string newBillRate, int index)
        {
            Contract.Rates.ElementAt(index).BillRate = float.Parse(newBillRate);
            UpdateGrossMarginAtIndex(index);
        }

        public string PayRateAtIndex(int index) { return Contract.Rates.ElementAt(index).PayRate.ToString(); }
        public void SetPayRateAtIndex(string newPayRate, int index)
        {
            Contract.Rates.ElementAt(index).PayRate = float.Parse(newPayRate);
            UpdateGrossMarginAtIndex(index);
        }

        public string GrossMarginAtIndex(int index) { return Contract.Rates.ElementAt(index).GrossMargin.ToString("0.00"); }
        public void SetGrossMarginAtIndex(string newGM, int index)
        {
            if( index < Contract.Rates.Count() )
                Contract.Rates.ElementAt(index).GrossMargin = float.Parse(newGM);
        }
        private void UpdateGrossMarginAtIndex( int index )
        {
            if (index >= Contract.Rates.Count()) return;
            
            float billRate = float.Parse(BillRateAtIndex(index));
            float payRate = float.Parse(PayRateAtIndex(index));

            if (billRate == 0) return;

            var GM = billRate - payRate;
            SetGrossMarginAtIndex(GM.ToString(), index);
        }

        public int PrimaryRateIndex {
            get { return Contract.PrimaryRateIndex;  }
        }
        public bool IsPrimaryRateAtIndex(int index) { return Contract.Rates.ElementAt(index).IsPrimaryRate; }
        public void SetPrimaryRateForIndex(int index)
        {
            Contract.PrimaryRateIndex = index;

            for (int i = 0; i < NumRates; i ++)
            {
                if (i == index)
                    Contract.Rates.ElementAt(i).IsPrimaryRate = true;
                else
                    Contract.Rates.ElementAt(i).IsPrimaryRate = false;
            }
        }

        public int NumRates = 0;

        public void AddRate()
        {
            if (Contract.PrimaryRateIndex > NumRates)
                Contract.PrimaryRateIndex = 0;

            var rate = new Rate();

            if (NumRates > 0)
                rate.RateType = RateTypeAtIndex(0);
            else
                rate.RateType = string.Empty;
    
            rate.Description = string.Empty;
            rate.BillRate = 0;

            if (NumRates == 0)
                rate.IsPrimaryRate = true;

            var rateList = Contract.Rates.ToList();
            rateList.Add(rate);
            Contract.Rates = rateList.AsEnumerable();



            NumRates ++;
        }

        public void DeleteRate( int index )
        {
            if (index >= 0 && index < NumRates)
            {
                var rateList = Contract.Rates.ToList();
                rateList.RemoveAt(index);

                if (NumRates > 1)
                    Contract.Rates = rateList.AsEnumerable();
                else
                    Contract.Rates = Enumerable.Empty<Rate>();

                NumRates--;

                if (PrimaryRateIndex == index && NumRates > 0)
                    SetPrimaryRateForIndex(0);
                else if (PrimaryRateIndex > index && NumRates > 0)
                    SetPrimaryRateForIndex(PrimaryRateIndex - 1);
            }
        }

        //Sending
        public bool IsSendingConsultantContract{
            get { return Contract.IsSendingConsultantContract; }
            set { Contract.IsSendingConsultantContract = value; } }

        public int ClientContactId {
            get { return Contract.ClientContact.Id; }
        }
        public int BillingContactId
        {
            get { return Contract.BillingContact.Id; }
        }
        public int DirectReportId
        {
            get { return Contract.DirectReport.Id; }
        }

        public string ClientContactName
        {
            get { return Contract.ClientContact.FullName; }
        }
        public void SetClientContact(InternalEmployee contact)
        {
            Contract.ClientContact = contact;
        }


        public string DirectReportName
        {
            get { return Contract.DirectReport.FullName; }
        }
        public void SetDirectReport(InternalEmployee contact)
        {
            Contract.DirectReport = contact;
        }

        public string BillingContactName
        {
            get { return Contract.BillingContact.FullName; }
        }
        public void SetBillingContact(InternalEmployee contact)
        {
            Contract.BillingContact = contact;
        }

        public string InvoiceRecipientNameAtIndex(int index)
        {
            return Contract.InvoiceRecipients.ElementAt(index).FullName;
        }
        /*
        public string InvoiceRecipientCompanyAtIndex(int index)
        {
            return Contract.InvoiceRecipients.ElementAt(index).ClientName;
        }
        public string InvoiceRecipientEmailAtIndex(int index)
        {
            if (Contract.InvoiceRecipients.ElementAt(index).EmailAddresses != null && Contract.InvoiceRecipients.ElementAt(index).EmailAddresses.Count() > 0 )
                return Contract.InvoiceRecipients.ElementAt(index).EmailAddresses.ElementAt(0).Email;
            return string.Empty;
        }*/
        public void SetInvoiceRecipientAtIndex(InternalEmployee contact, int index)
        {
            if (Contract.InvoiceRecipients == null)
                Contract.InvoiceRecipients = new List<InternalEmployee>();

            List < InternalEmployee > recipientList = Contract.InvoiceRecipients.ToList();

            //add if doesnt exist
            if (index > Contract.InvoiceRecipients.Count()-1)
                recipientList.Add(contact);
            else
                recipientList[index] = contact;

            Contract.InvoiceRecipients = recipientList.AsEnumerable();
        }


        public string ClientContractContactName
        {
            get { return Contract.ClientContractContact.FullName; }
        }
        public void SetClientContractContact(InternalEmployee contact)
        {
            Contract.ClientContractContact = contact;
        }

        public bool IsSendingContractToClientContact
        {
            get { return Contract.IsSendingClientContract; }
            set { Contract.IsSendingClientContract = value; }
        }

        public string ReasonForNotSendingContract
        {
            get { return Contract.ReasonForNotSendingContract; }
            set { Contract.ReasonForNotSendingContract = value; }
        }

        public string SummaryReasonForNotSendingContract
        {
            get { return Contract.SummaryReasonForNotSendingContract; }
            set { Contract.SummaryReasonForNotSendingContract = value; }
        }

        public string SummaryReasonForNotSendingConsultantContract
        {
            get { return Contract.SummaryReasonForNotSendingConsultantContract; }
            set { Contract.SummaryReasonForNotSendingConsultantContract = value; }
        }
        
        #region SupportData

        public List<string> BooleanOptions { get { return new List<string>(new string[] { "Yes", "No" }); } }

        public int BooleanOptionIndex( bool value )
        {
            if (value == true)
                return 0;
            return 1;
        }


        #endregion


        private int IndexSelectionFromOptions( List<string> options, string value )
        {
            if( options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });
           
            return 0;
        }

        private int IndexBooleanSelectionFromOptions(List<string> options, bool booleanValue)
        {
            string value = BooleanOptions[0];

            if (booleanValue == false)
                value = BooleanOptions[1];

            if (options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });

            return 0;
        }

        public Task LoadContractCreationInitialPageData()
        {

            var task = GetContractCreationInitialPageDetails( JobId, CandidateId);

            return task;
        }

        public Task LoadContractCreationPayRatePageData()
        {

            var secondTask = GetContractCreationPayRatePageDetails();

            return secondTask;
        }

        public Task LoadContractCreationSendingPageData()
        {

            var secondTask = GetContractCreationSendingPageDetails();

            return secondTask;
        }



        private async Task GetContractCreationInitialPageDetails( int jobId, int candidateId )
        {
            _contract = await _api.GetInitialContract(jobId, candidateId);
        }


        private async Task GetContractCreationSendingPageDetails()
        {
            //ContractCreationDetails_Sending sendingDetails = await _api.GetContractCreationSendingPageDetails(JobId, CandidateId);
        }


        private async Task GetContractCreationPayRatePageDetails()
        {
            // _contract = await _api.GetContractRatesPageDetails( JobId, CandidateId);
            
        }

        public async Task SubmitContract()
        {

        }
    }
}