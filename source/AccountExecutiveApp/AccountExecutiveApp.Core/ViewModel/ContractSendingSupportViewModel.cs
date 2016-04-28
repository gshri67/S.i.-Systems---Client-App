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

        private ContractCreationOptions_Sending _options;
        private ContractCreationOptions_Sending Options
        {
            get { return _options ?? new ContractCreationOptions_Sending(); }
            set { _options = value ?? new ContractCreationOptions_Sending(); }
        }

        public ContractSendingSupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        //public int IsSendingConsultantContractSelectedIndex { get { return IndexBooleanSelectionFromOptions(BooleanOptions, IsSendingConsultantContract); } }

        public List<InternalEmployee> ClientContactOptions
        {
            get
            {
                return _options.ClientContactOptions;
            }
        }
        public List<string> ClientContactNameOptions { get { return ClientContactOptions.Select(c => c.FullName).ToList(); } }

        public InternalEmployee GetClientContactWithName(string name)
        {
            return ClientContactOptions.FirstOrDefault(c => c.FullName == name);
        }

        public List<InternalEmployee> DirectReportOptions
        {
            get
            {
                return _options.DirectReportOptions;
            }
        }
        public List<string> DirectReportNameOptions { get { return DirectReportOptions.Select(c => c.FullName).ToList(); } }

        public InternalEmployee GetDirectReportWithName(string name)
        {
            return DirectReportOptions.FirstOrDefault(c => c.FullName == name);
        }

        public List<InternalEmployee> BillingContactOptions
        {
            get
            {
                return _options.BillingContactOptions;
            }
        }
        public List<string> BillingContactNameOptions { get { return BillingContactOptions.Select(c => c.FullName).ToList(); } }

        public InternalEmployee GetBillingContactWithName(string name)
        {
            return BillingContactOptions.FirstOrDefault(c => c.FullName == name);
        }

        public List<InternalEmployee> ClientContractContactOptions
        {
            get
            {
                InternalEmployee contact1 = new InternalEmployee { FirstName = "Candice", LastName = "Consulty", Id = 1 };
                InternalEmployee contact2 = new InternalEmployee { FirstName = "Jessica", LastName = "Wu", Id = 2 };
                return new List<InternalEmployee>(new InternalEmployee[] { contact1, contact2 });
            }
        }
        public List<string> ClientContractContactNameOptions { get { return ClientContractContactOptions.Select(c => c.FullName).ToList(); } }

        public InternalEmployee GetClientContractContactWithName(string name)
        {
            return ClientContractContactOptions.FirstOrDefault(c => c.FullName == name);
        }

        public List<InternalEmployee> InvoiceRecipientOptions
        {
            get
            {
                InternalEmployee contact1 = new InternalEmployee { FirstName = "Candice", LastName = "Consulty", Id = 1 };
                InternalEmployee contact2 = new InternalEmployee { FirstName = "Jessica", LastName = "Wu", Id = 2 };
                return new List<InternalEmployee>(new InternalEmployee[] { contact1, contact2 });
            }
        }

        public string[] InvoiceRecipientOptionNames
        {
            get { return InvoiceRecipientOptions.Select(c => c.FullName).ToArray<string>(); }
        }
        public InternalEmployee GetInvoiceRecipientWithName(string name)
        {
            return InvoiceRecipientOptions.FirstOrDefault(c => c.FullName == name);
        }
        
        public List<string> ReasonForNotSendingContractOptions { get { return new List<string>(new string[] { "Client has a master agreement(MSA, VMS, etc.)", "Client has provided their own contract", "Other" }); } }
 
        public Task GetContractSupportOptions(int jobId, int candidateId)
        {
            var task = GetOptions( jobId );
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions( int jobId )
        {
            Options = await _api.GetDropDownValuesForContractCreationSendingForm(jobId);
        }
    }
}