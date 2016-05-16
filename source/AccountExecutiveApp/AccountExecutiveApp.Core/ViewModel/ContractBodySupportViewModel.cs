using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractBodySupportViewModel
    {
        private readonly IMatchGuideApi _api;
  
        private ContractCreationOptions _contractOptions;
        private ContractCreationOptions ContractOptions
        {
            get { return _contractOptions ?? new ContractCreationOptions(); }
            set { _contractOptions = value ?? new ContractCreationOptions(); }
        }
        
        public List<string> BooleanOptions { get { return new List<string>(new string[] { "Yes", "No" }); } }

        public int BooleanOptionIndex( bool value )
        {
            if (value == true)
                return 0;
            return 1;
        }

        public ContractBodySupportViewModel(IMatchGuideApi api )
        {
            _api = api;
        }


        public List<string> TimeFactorOptions { get { return ContractOptions.TimeFactorOptions.ToList(); } }
        public List<string> LimitationExpenseOptions { get { return ContractOptions.LimitationExpenseOptions.ToList(); } }
        public List<string> LimitationOfContractTypeOptions { get { return ContractOptions.LimitationOfContractTypeOptions.ToList(); } }
        public List<string> PaymentPlanOptions { get { return ContractOptions.PaymentPlanOptions.ToList(); } }
        public List<string> DaysCancellationOptions { get { return ContractOptions.DaysCancellationOptions.ToList(); } }

        public List<string> InvoiceFrequencyOptions { get { return ContractOptions.InvoiceFrequencyOptions.ToList(); } }
        public List<string> InvoiceFormatOptions { get { return ContractOptions.InvoiceFormatOptions.ToList(); } }

        private List<InternalEmployee> AccountExecutiveOptions
        {
            get { return ContractOptions.Colleagues.ToList(); }
            /*
            get
            {
                return new List<InternalEmployee>(new InternalEmployee[] { new InternalEmployee
            {
                Id = 1,
                FirstName = "Robert",
                LastName = "Paulson"
            },
            new InternalEmployee
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith"
            },
            new InternalEmployee
            {
                Id = 3,
                FirstName = "Fred",
                LastName = "Flintstone"
            }
        });
            }*/
        }
        public List<string> AccountExecutiveOptionDescriptions
        {
            get { return AccountExecutiveOptions.Select(c => c.FullName).ToList(); }
        }

        public InternalEmployee GetAccountExecutiveWithName(string name)
        {
            return AccountExecutiveOptions.FirstOrDefault(c => c.FullName == name);
        }

        private List<InternalEmployee> GMAssignedOptions
        {
            get { return ContractOptions.Colleagues.ToList(); }
        }

        public List<string> GMAssignedOptionDescriptions
        {
            get { return GMAssignedOptions.Select(c => c.FullName).ToList(); }
        }

        public InternalEmployee GetGMAssignedWithName(string name)
        {
            return GMAssignedOptions.FirstOrDefault(c => c.FullName == name);
        }

        private List<InternalEmployee> ComissionAssignedOptions
        {
            get { return ContractOptions.Colleagues.ToList(); }
        }

        public List<string> ComissionAssignedOptionDescriptions
        {
            get { return ComissionAssignedOptions.Select(c => c.FullName).ToList(); }
        }

        public List<string> BranchOptions {
            get { return ContractOptions.BranchOptions.ToList(); }
        }

        public InternalEmployee GetComissionAssignedWithName(string name)
        {
            return ComissionAssignedOptions.FirstOrDefault(c => c.FullName == name);
        }


        public Task GetContractBodyOptions( int jobId )
        {
            var task = GetOptions( jobId );
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions( int jobId )
        {
            ContractOptions = await _api.GetDropDownValuesForInitialContractCreationForm( jobId );
        }

        public Task UpdateColleaguesWithBranch( int branch )
        {
            var task = GetColleaguesWithBranchFromAPI( branch );
            //todo: task.continueWith
            return task;
        }

        private async Task GetColleaguesWithBranchFromAPI( int branch )
        {
            var list = await _api.GetDropDownValuesForColleaguesWithBranch( branch );
            
            if (list != null && list.Count() > 0)
                ContractOptions.Colleagues = list;
        }
    }
}