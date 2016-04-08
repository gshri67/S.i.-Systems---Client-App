using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using SiSystems.SharedModels.Contract_Creation;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractSendingSupportViewModel
    {
        private readonly IMatchGuideApi _api;

        private SendingOptions _options;
        private SendingOptions Options
        {
            get { return _options ?? new SendingOptions(); }
            set { _options = value ?? new SendingOptions(); }
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

        public UserContact GetClientContactWithName(string name)
        {
            return ClientContactOptions.FirstOrDefault(c => c.FullName == name);
        }

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

        public UserContact GetDirectReportWithName(string name)
        {
            return DirectReportOptions.FirstOrDefault(c => c.FullName == name);
        }

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

        public UserContact GetBillingContactWithName(string name)
        {
            return BillingContactOptions.FirstOrDefault(c => c.FullName == name);
        }

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

        public UserContact GetClientContractContactWithName(string name)
        {
            return ClientContractContactOptions.FirstOrDefault(c => c.FullName == name);
        }

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
        public UserContact GetInvoiceRecipientWithName(string name)
        {
            return InvoiceRecipientOptions.FirstOrDefault(c => c.FullName == name);
        }
        
        public List<string> ReasonForNotSendingContractOptions { get { return new List<string>(new string[] { "Client has a master agreement(MSA, VMS, etc.)", "Client has provided their own contract", "Other" }); } }
 
        public Task GetContractBodyOptions(int jobId, int candidateId)
        {
            var task = GetOptions();
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions()
        {
            //Options = await _api.GetDropDownValuesForSendingContractCreationForm();
        }
    }
}