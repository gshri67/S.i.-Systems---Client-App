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

        //First Page
        public DateTime StartDate {
                                                get { return Contract.StartDate; }
                                                set { Contract.StartDate = value; } }
        public DateTime EndDate
        {
            get { return Contract.EndDate; }
            set { Contract.EndDate = value; }
        }

        public string JobTitle {                get { return Contract.JobTitle; }
                                                set { Contract.JobTitle = value; }}

        public string TimeFactor {              get { return Contract.TimeFactor; }
                                                set { Contract.TimeFactor = value; }}
        public int DaysCancellation {           get { return Contract.DaysCancellation; }
                                                set { Contract.DaysCancellation = value; }}
        public List<string> DaysCancellationOptions { get { return new List<string>(new string[] { "5", "10", "15" }); } }
        public int DaysCancellationSelectedIndex { get { return IndexSelectionFromOptions(DaysCancellationOptions, DaysCancellation.ToString()); } }

        public string LimitationExpense {       get { return Contract.LimitationExpense; }
                                                set { Contract.LimitationExpense = value; }}
        public string LimitationOfContract {    get { return Contract.LimitationOfContract; } 
                                                set { Contract.LimitationOfContract = value; } }

        public string PaymentPlan {             get { return Contract.PaymentPlan; }
                                                set { Contract.PaymentPlan = value; }}
        public UserContact AccountExecutive {   get { return Contract.AccountExecutive; }
                                                set { Contract.AccountExecutive = value; }}

        public int AccountExecutiveIndex { get { return IndexSelectionFromOptions(AccountExecutiveOptionDescriptions, AccountExecutive.FullName); } }
        public void SetAccountExecutiveWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(AccountExecutiveOptionDescriptions, newValue);
            AccountExecutive = AccountExecutiveOptions[selectedIndex];
        }

        public UserContact GMAssigned {         get { return Contract.GMAssigned; } 
                                                set { Contract.GMAssigned = value; } }

        public int GMAssignedIndex { get { return IndexSelectionFromOptions(GMAssignedOptionDescriptions, GMAssigned.FullName); } }
        public void SetGMAssignedWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(GMAssignedOptionDescriptions, newValue);
            GMAssigned = GMAssignedOptions[selectedIndex];
        }

        public UserContact ComissionAssigned {  get { return Contract.ComissionAssigned; } 
                                                set { Contract.ComissionAssigned = value; } }

        public int ComissionAssignedIndex { get { return IndexSelectionFromOptions(ComissionAssignedOptionDescriptions, ComissionAssigned.FullName); } }
        public void SetComissionAssignedWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(ComissionAssignedOptionDescriptions, newValue);
            ComissionAssigned = ComissionAssignedOptions[selectedIndex];
        }

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
            Contract.Rates.ElementAt(index).RateType = newRateType;
        }

        public string RateDescriptionAtIndex(int index) { return Contract.Rates.ElementAt(index).RateDescription; }
        public void SetRateDescriptionAtIndex(string newRateDescription, int index)
        {
            Contract.Rates.ElementAt(index).RateDescription = newRateDescription;
        }

        public string BillRateAtIndex(int index) { return Contract.Rates.ElementAt(index).BillRate.ToString(); }
        public void SetBillRateAtIndex(string newBillRate, int index)
        {
            Contract.Rates.ElementAt(index).BillRate = float.Parse(newBillRate);
        }

        public int PrimaryRateIndex {
            get { return Contract.PrimaryRateIndex;  }
        }
        public bool IsPrimaryRateAtIndex(int index) { return Contract.Rates.ElementAt(index).isPrimaryRate; }
        public void SetPrimaryRateForIndex(int index)
        {
            Contract.PrimaryRateIndex = index;

            for (int i = 0; i < NumRates; i ++)
            {
                if (i == index)
                    Contract.Rates.ElementAt(i).isPrimaryRate = true;
                else
                    Contract.Rates.ElementAt(i).isPrimaryRate = false;
            }
        }

        public int NumRates = 0;

        public void AddRate()
        {
            if (Contract.PrimaryRateIndex > NumRates)
                Contract.PrimaryRateIndex = 0;


            ContractCreationDetails_Rate rate = new ContractCreationDetails_Rate();
            rate.RateType = string.Empty;
            rate.RateDescription = string.Empty;
            rate.BillRate = 0;

            if (NumRates == 1)
                rate.isPrimaryRate = true;

            List<ContractCreationDetails_Rate> rateList = Contract.Rates.ToList();
            rateList.Add(rate);
            Contract.Rates = rateList.AsEnumerable();

            NumRates ++;
        }

        public void DeleteRate( int index )
        {
            if (index >= 0 && index < NumRates)
            {
                List<ContractCreationDetails_Rate> rateList = Contract.Rates.ToList();
                rateList.RemoveAt(index);

                if (NumRates > 1)
                    Contract.Rates = rateList.AsEnumerable();
                else
                    Contract.Rates = Enumerable.Empty<ContractCreationDetails_Rate>();

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

        public string ClientContactName
        {
            get { return Contract.ClientContact.FullName; }
        }
        public void SetClientContact(UserContact contact)
        {
            Contract.ClientContact = contact;
        }
        public UserContact GetClientContactWithName(string name)
        {
            return ClientContactOptions.FirstOrDefault(c => c.FullName == name);
        }

        public string DirectReportName
        {
            get { return Contract.DirectReport.FullName; }
        }
        public void SetDirectReport(UserContact contact)
        {
            Contract.DirectReport = contact;
        }
        public UserContact GetDirectReportWithName(string name)
        {
            return DirectReportOptions.FirstOrDefault(c => c.FullName == name);
        }

        public string BillingContactName
        {
            get { return Contract.BillingContact.FullName; }
        }
        public void SetBillingContact(UserContact contact)
        {
            Contract.BillingContact = contact;
        }
        public UserContact GetBillingContactWithName(string name)
        {
            return BillingContactOptions.FirstOrDefault(c => c.FullName == name);
        }

        public string InvoiceRecipientNameAtIndex(int index)
        {
            return Contract.InvoiceRecipients.ElementAt(index).FullName;
        }
        public string InvoiceRecipientCompanyAtIndex(int index)
        {
            return Contract.InvoiceRecipients.ElementAt(index).ClientName;
        }
        public string InvoiceRecipientEmailAtIndex(int index)
        {
            if (Contract.InvoiceRecipients.ElementAt(index).EmailAddresses != null && Contract.InvoiceRecipients.ElementAt(index).EmailAddresses.Count() > 0 )
                return Contract.InvoiceRecipients.ElementAt(index).EmailAddresses.ElementAt(0).Email;
            return string.Empty;
        }
        public void SetInvoiceRecipientAtIndex(UserContact contact, int index)
        {
            if (Contract.InvoiceRecipients == null)
                Contract.InvoiceRecipients = new List<UserContact>();

            List < UserContact > recipientList = Contract.InvoiceRecipients.ToList();

            //add if doesnt exist
            if (index > Contract.InvoiceRecipients.Count()-1)
                recipientList.Add(contact);
            else
                recipientList[index] = contact;

            Contract.InvoiceRecipients = recipientList.AsEnumerable();
        }
        public UserContact GetInvoiceRecipientWithName(string name)
        {
            return InvoiceRecipientOptions.FirstOrDefault(c => c.FullName == name);
        }

        public string ClientContractContactName
        {
            get { return Contract.ClientContractContact.FullName; }
        }
        public void SetClientContractContact(UserContact contact)
        {
            Contract.ClientContractContact = contact;
        }
        public UserContact GetClientContractContactWithName(string name)
        {
            return ClientContractContactOptions.FirstOrDefault(c => c.FullName == name);
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

        #region SupportData

        public List<string> BooleanOptions { get { return new List<string>(new string[] { "Yes", "No" }); } }

        public int BooleanOptionIndex( bool value )
        {
            if (value == true)
                return 0;
            return 1;
        }

        //First Page

        public List<string> TimeFactorOptions { get { return new List<string>(new string[] { "Full Time", "Half Time", "Part Time" }); } }
        public int TimeFactorSelectedIndex { get { return IndexSelectionFromOptions(TimeFactorOptions, TimeFactor); } }

        public List<string> LimitationExpenseOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationExpenseSelectedIndex { get { return IndexSelectionFromOptions(LimitationExpenseOptions, LimitationExpense); } }

        public List<string> LimitationOfContractOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationOfContractSelectedIndex { get { return IndexSelectionFromOptions(LimitationOfContractOptions, LimitationOfContract); } }

        public List<string> PaymentPlanOptions { get { return new List<string>(new string[] { "Monthly Standard Last Business Day", "half-time", "part-time" }); } }
        public int PaymentPlanSelectedIndex { get { return IndexSelectionFromOptions(PaymentPlanOptions, PaymentPlan); } }


        private List<UserContact> AccountExecutiveOptions
        {
            get
            {
                return new List<UserContact>(new UserContact[] { new UserContact
            {
                Id = 1,
                FirstName = "Robert",
                LastName = "Paulson"
            },
            new UserContact
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith"
            },
            new UserContact
            {
                Id = 3,
                FirstName = "Fred",
                LastName = "Flintstone"
            }
        });
            }
        }
        public List<string> AccountExecutiveOptionDescriptions
        {
            get { return AccountExecutiveOptions.Select(c => c.FullName).ToList(); }
        }

        private List<UserContact> GMAssignedOptions
        {
            get
            {
                return new List<UserContact>(new UserContact[] { new UserContact
            {
                Id = 1,
                FirstName = "Robert",
                LastName = "Paulson"
            },
            new UserContact
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith"
            },
            new UserContact
            {
                Id = 3,
                FirstName = "Fred",
                LastName = "Flintstone"
            }
        });
            }
        }

        public List<string> GMAssignedOptionDescriptions
        {
            get { return GMAssignedOptions.Select(c => c.FullName).ToList(); }
        }

        private List<UserContact> ComissionAssignedOptions
        {
            get
            {
                return new List<UserContact>(new UserContact[] { new UserContact
            {
                Id = 1,
                FirstName = "Robert",
                LastName = "Paulson"
            },
            new UserContact
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith"
            },
            new UserContact
            {
                Id = 3,
                FirstName = "Fred",
                LastName = "Flintstone"
            }
        });
            }
        }

        public List<string> ComissionAssignedOptionDescriptions
        {
            get { return ComissionAssignedOptions.Select(c => c.FullName).ToList(); }
        }

        public List<string> InvoiceFrequencyOptions { get { return new List<string>(new string[] { "Monthly", "Semi Monthly", "Weekly Invoicing" }); } }
        public int InvoiceFrequencySelectedIndex { get { return IndexSelectionFromOptions(InvoiceFrequencyOptions, InvoiceFrequency); } }

        public List<string> InvoiceFormatOptions { get { return new List<string>(new string[] { "1 invoice per contract" }); } }
        public int InvoiceFormatSelectedIndex { get { return IndexSelectionFromOptions(InvoiceFormatOptions, InvoiceFormat); } }

        //Pay Rates
        public List<string> RateTypeOptions { get { return new List<string>(new string[] { "Per hour", "Per day" }); } }
        public int RateTypeSelectedIndexAtIndex(int index) { return IndexSelectionFromOptions(RateTypeOptions, RateTypeAtIndex(index)); }

        public List<string> RateDescriptionOptions { get { return new List<string>(new string[] { "Regular", "Hourly", "Daily" }); } }

        public int RateDescriptionSelectedIndexAtIndex(int index) { return IndexSelectionFromOptions(RateDescriptionOptions, RateDescriptionAtIndex(index)); }

        //Sending
        public int IsSendingConsultantContractSelectedIndex { get { return IndexBooleanSelectionFromOptions(BooleanOptions, IsSendingConsultantContract); } }

        public List<UserContact> ClientContactOptions
        {
            get
            {
                UserContact contact1 = new UserContact{ FirstName = "Candice", LastName = "Consulty"};
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu" };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }
        public List<string> ClientContactNameOptions { get { return ClientContactOptions.Select(c => c.FullName).ToList(); } }

        public int ClientContactNameSelectedIndex { get { return IndexSelectionFromOptions(ClientContactNameOptions, ClientContactName); } }

        public List<UserContact> DirectReportOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty" };
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu" };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }
        public List<string> DirectReportNameOptions { get { return DirectReportOptions.Select(c => c.FullName).ToList(); } }
        public int DirectReportNameSelectedIndex { get { return IndexSelectionFromOptions(DirectReportNameOptions, DirectReportName); } }

        public List<UserContact> BillingContactOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty" };
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu" };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }
        public List<string> BillingContactNameOptions { get { return BillingContactOptions.Select(c => c.FullName).ToList(); } }
        public int BillingContactNameSelectedIndex { get { return IndexSelectionFromOptions(BillingContactNameOptions, BillingContactName); } }

        public List<UserContact> ClientContractContactOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty" };
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu" };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }
        public List<string> ClientContractContactNameOptions { get { return ClientContractContactOptions.Select(c => c.FullName).ToList(); } }
        public int ClientContractContactNameSelectedIndex { get { return IndexSelectionFromOptions(ClientContractContactNameOptions, ClientContractContactName); } }

        public List<UserContact> InvoiceRecipientOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty", Id = 1};
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu", Id = 2 };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }

        public string[] InvoiceRecipientOptionNames {
            get { return InvoiceRecipientOptions.Select(c => c.FullName).ToArray<string>(); }
        }

        public List<string> ReasonForNotSendingContractOptions { get { return new List<string>(new string[] { "Client has a master agreement(MSA, VMS, etc.)", "Client has provided their own contract", "Other" }); } }
        public int ReasonForNotSendingContractSelectedIndex { get { return IndexSelectionFromOptions(ReasonForNotSendingContractOptions, ReasonForNotSendingContract); } }

        public int IsSendingContractToClientContactSelectedIndex { get { return IndexBooleanSelectionFromOptions(BooleanOptions, IsSendingContractToClientContact); } }
        

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
            var firstTask = GetContractCreationInitialPageOptions();
            var secondTask = GetContractCreationInitialPageDetails();

            return secondTask;
        }

        public Task LoadContractCreationPayRatePageData()
        {
            var firstTask = GetContractCreationPayRatePageOptions();
            var secondTask = GetContractCreationPayRatePageDetails();

            return secondTask;
        }

        public Task LoadContractCreationSendingPageData()
        {
            var firstTask = GetContractCreationSendingPageOptions();
            var secondTask = GetContractCreationSendingPageDetails();

            return secondTask;
        }


        private async Task GetContractCreationInitialPageOptions()
        {
            //ContractCreationOptions initialOptions = await _api.GetContractCreationInitialPageOptions(JobId, CandidateId);
        }
        private async Task GetContractCreationInitialPageDetails()
        {
            //ContractCreationDetails initialDetails = await _api.GetContractCreationInitialPageDetails(JobId, CandidateId);
        }

        private async Task GetContractCreationSendingPageOptions()
        {
            //ContractCreationOptions_Sending sendingOptions = await _api.GetContractCreationSendingPageOptions(JobId, CandidateId);
        }
        private async Task GetContractCreationSendingPageDetails()
        {
            //ContractCreationDetails_Sending sendingDetails = await _api.GetContractCreationSendingPageDetails(JobId, CandidateId);
        }

        private async Task GetContractCreationPayRatePageOptions()
        {
            //ContractCreationOptions_Rate rateOptions = await _api.GetContractCreationPayRatePageOptions(JobId, CandidateId);
        }
        private async Task GetContractCreationPayRatePageDetails()
        {
            //ContractCreationDetails_Rate rateDetails = await _api.GetContractCreationPayRatePageDetails(JobId, CandidateId);
        }
    }
}