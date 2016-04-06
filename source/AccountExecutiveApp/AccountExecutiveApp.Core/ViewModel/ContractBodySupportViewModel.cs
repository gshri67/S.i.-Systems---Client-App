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

        public ContractBodySupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }


        public List<string> TimeFactorOptions { get { return new List<string>(new string[] { "Full Time", "Half Time", "Part Time" }); } }
        public List<string> LimitationExpenseOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public List<string> LimitationOfContractTypeOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public List<string> PaymentPlanOptions { get { return new List<string>(new string[] { "Monthly Standard Last Business Day", "half-time", "part-time" }); } }
        public List<string> DaysCancellationOptions { get { return new List<string>(new string[] { "5", "10", "15" }); } }

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
        public List<string> InvoiceFormatOptions { get { return new List<string>(new string[] { "1 invoice per contract" }); } }

        public Task GetContractBodyOptions()
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