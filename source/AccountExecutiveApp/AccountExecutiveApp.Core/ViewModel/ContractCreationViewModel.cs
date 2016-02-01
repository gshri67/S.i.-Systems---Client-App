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
        private ContractCreationDetails Contract
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


        public DateTime StartDate {               set { Contract.StartDate = value; }}
        public DateTime EndDate {                 set { Contract.EndDate = value; } }

        public string JobTitle {                get { return Contract.JobTitle; }
                                                set { Contract.JobTitle = value; }}
        public string TimeFactor {              get { return Contract.TimeFactor; }
                                                set { Contract.TimeFactor = value; }}
        public int DaysCancellation {           get { return Contract.DaysCancellation; }
                                                set { Contract.DaysCancellation = value; }}
        public string LimitationExpense {       get { return Contract.LimitationExpense; }
                                                set { Contract.LimitationExpense = value; }}
        public string LimitationOfContract {    get { return Contract.LimitationOfContract; } 
                                                set { Contract.LimitationOfContract = value; } }
        public string PaymentPlan {             get { return Contract.PaymentPlan; }
                                                set { Contract.PaymentPlan = value; }}
        public UserContact AccountExecutive {   get { return Contract.AccountExecutive; }
                                                set { Contract.AccountExecutive = value; }}
        public UserContact GMAssigned {         get { return Contract.GMAssigned; } 
                                                set { Contract.GMAssigned = value; } }
        public UserContact ComissionAssigned {  get { return Contract.ComissionAssigned; } 
                                                set { Contract.ComissionAssigned = value; } }
        public string InvoiceFrequency {        get { return Contract.InvoiceFrequency; } 
                                                set { Contract.InvoiceFrequency = value; } }
        public string InvoiceFormat {           get { return Contract.InvoiceFormat; } 
                                                set { Contract.InvoiceFormat = value; } }
        public Boolean UsingProjectCode {       get { return Contract.UsingProjectCode; }
                                                set { Contract.UsingProjectCode = value; }}
        public Boolean UsingQuickPay {          get { return Contract.UsingQuickPay; } 
                                                set { Contract.UsingQuickPay = value; } }

        /*
        public Task LoadContract(int Id)
        {
            var task = GetContract(Id);

            return task;
        }*/



    }
}