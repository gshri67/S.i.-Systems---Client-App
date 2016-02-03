using System;
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


        public ContractCreationViewModel(IMatchGuideApi api)
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
        public List<string> TimeFactorOptions { get { return new List<string>(new string[] { "full-time", "half-time", "part-time" }); } }
        public int TimeFactorSelectedIndex {    get { return IndexSelectionFromOptions(TimeFactorOptions, TimeFactor); } }

        public int DaysCancellation {           get { return Contract.DaysCancellation; }
                                                set { Contract.DaysCancellation = value; }}
        public List<string> DaysCancellationOptions { get { return new List<string>(new string[] { "5", "10", "15" }); } }
        public int DaysCancellationSelectedIndex { get { return IndexSelectionFromOptions(DaysCancellationOptions, DaysCancellation.ToString()); } }

        public string LimitationExpense {       get { return Contract.LimitationExpense; }
                                                set { Contract.LimitationExpense = value; }}
        public List<string> LimitationExpenseOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationExpenseSelectedIndex {    get { return IndexSelectionFromOptions(LimitationExpenseOptions, LimitationExpense); } }

        public string LimitationOfContract {    get { return Contract.LimitationOfContract; } 
                                                set { Contract.LimitationOfContract = value; } }
        public List<string> LimitationOfContractOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationOfContractSelectedIndex { get { return IndexSelectionFromOptions(LimitationOfContractOptions, LimitationOfContract); } }


        public string PaymentPlan {             get { return Contract.PaymentPlan; }
                                                set { Contract.PaymentPlan = value; }}
        public List<string> PaymentPlanOptions {get { return new List<string>(new string[] { "Monthly Standard", "half-time", "part-time" }); } }
        public int PaymentPlanSelectedIndex {   get { return IndexSelectionFromOptions(PaymentPlanOptions, PaymentPlan); } }

        public UserContact AccountExecutive {   get { return Contract.AccountExecutive; }
                                                set { Contract.AccountExecutive = value; }}
        private List<UserContact> AccountExecutiveOptions { get { return new List<UserContact>(new UserContact[] { new UserContact
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
        }); } }
        public List<string> AccountExecutiveOptionDescriptions
        {
            get { return  AccountExecutiveOptions.Select( c => c.FullName ).ToList(); }
        }

        public int AccountExecutiveIndex { get { return IndexSelectionFromOptions(AccountExecutiveOptionDescriptions, AccountExecutive.FullName); } }
        public void SetAccountExecutiveWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(AccountExecutiveOptionDescriptions, newValue);
            AccountExecutive = AccountExecutiveOptions[selectedIndex];
        }

        public UserContact GMAssigned {         get { return Contract.GMAssigned; } 
                                                set { Contract.GMAssigned = value; } }
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

        public int GMAssignedIndex { get { return IndexSelectionFromOptions(GMAssignedOptionDescriptions, GMAssigned.FullName); } }
        public void SetGMAssignedWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(GMAssignedOptionDescriptions, newValue);
            GMAssigned = GMAssignedOptions[selectedIndex];
        }

        public UserContact ComissionAssigned {  get { return Contract.ComissionAssigned; } 
                                                set { Contract.ComissionAssigned = value; } }
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

        public int ComissionAssignedIndex { get { return IndexSelectionFromOptions(ComissionAssignedOptionDescriptions, ComissionAssigned.FullName); } }
        public void SetComissionAssignedWithName(string newValue)
        {
            int selectedIndex = IndexSelectionFromOptions(ComissionAssignedOptionDescriptions, newValue);
            ComissionAssigned = ComissionAssignedOptions[selectedIndex];
        }

        public string InvoiceFrequency {        get { return Contract.InvoiceFrequency; } 
                                                set { Contract.InvoiceFrequency = value; } }
        public List<string> InvoiceFrequencyOptions { get { return new List<string>(new string[] { "Semi-Monthly", "Monthly", "Bi-Weekly" }); } }
        public int InvoiceFrequencySelectedIndex { get { return IndexSelectionFromOptions(InvoiceFrequencyOptions, InvoiceFrequency); } }

        public string InvoiceFormat {           get { return Contract.InvoiceFormat; } 
                                                set { Contract.InvoiceFormat = value; } }
        public List<string> InvoiceFormatOptions { get { return new List<string>(new string[] { "1 invoice per contract" }); } }
        public int InvoiceFormatSelectedIndex { get { return IndexSelectionFromOptions(InvoiceFormatOptions, InvoiceFormat); } }

        public Boolean UsingProjectCode {       get { return Contract.UsingProjectCode; }
                                                set { Contract.UsingProjectCode = value; }}
        public Boolean UsingQuickPay {          get { return Contract.UsingQuickPay; } 
                                                set { Contract.UsingQuickPay = value; } }

        private int IndexSelectionFromOptions( List<string> options, string value )
        {
            if( options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });
           
            return 0;
        }
        /*
        public Task LoadContract(int Id)
        {
            var task = GetContract(Id);

            return task;
        }*/

    }
}