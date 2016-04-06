using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractSendingSupportViewModel
    {
        private readonly IMatchGuideApi _api;

        private ContractCreationOptions _contractOptions;
        private ContractCreationOptions ContractOptions
        {
            get { return _contractOptions ?? new ContractCreationOptions(); }
            set { _contractOptions = value ?? new ContractCreationOptions(); }
        }

        public ContractSendingSupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        //public int IsSendingConsultantContractSelectedIndex { get { return IndexBooleanSelectionFromOptions(BooleanOptions, IsSendingConsultantContract); } }

        public List<UserContact> ClientContactOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty" };
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu" };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }
        public List<string> ClientContactNameOptions { get { return ClientContactOptions.Select(c => c.FullName).ToList(); } }

       // public int ClientContactNameSelectedIndex { get { return IndexSelectionFromOptions(ClientContactNameOptions, ClientContactName); } }

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
        //public int DirectReportNameSelectedIndex { get { return IndexSelectionFromOptions(DirectReportNameOptions, DirectReportName); } }

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
        //public int BillingContactNameSelectedIndex { get { return IndexSelectionFromOptions(BillingContactNameOptions, BillingContactName); } }

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
        //public int ClientContractContactNameSelectedIndex { get { return IndexSelectionFromOptions(ClientContractContactNameOptions, ClientContractContactName); } }

        public List<UserContact> InvoiceRecipientOptions
        {
            get
            {
                UserContact contact1 = new UserContact { FirstName = "Candice", LastName = "Consulty", Id = 1 };
                UserContact contact2 = new UserContact { FirstName = "Jessica", LastName = "Wu", Id = 2 };
                return new List<UserContact>(new UserContact[] { contact1, contact2 });
            }
        }

        public string[] InvoiceRecipientOptionNames
        {
            get { return InvoiceRecipientOptions.Select(c => c.FullName).ToArray<string>(); }
        }

        public List<string> ReasonForNotSendingContractOptions { get { return new List<string>(new string[] { "Client has a master agreement(MSA, VMS, etc.)", "Client has provided their own contract", "Other" }); } }
        //public int ReasonForNotSendingContractSelectedIndex { get { return IndexSelectionFromOptions(ReasonForNotSendingContractOptions, ReasonForNotSendingContract); } }

        //public int IsSendingContractToClientContactSelectedIndex { get { return IndexBooleanSelectionFromOptions(BooleanOptions, IsSendingContractToClientContact); } }
        


        public Task GetContractBodyOptions(int jobId, int candidateId)
        {
            var task = GetOptions();
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions()
        {
            ContractOptions = await _api.GetDropDownValuesForInitialContractCreationForm();
        }
    }
}