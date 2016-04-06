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

        public ContractBodySupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }


        public List<string> TimeFactorOptions { get { return new List<string>(new string[] { "Full Time", "Half Time", "Part Time" }); } }
        public int TimeFactorSelectedIndex { get { return IndexSelectionFromOptions(TimeFactorOptions, TimeFactor); } }

        public List<string> LimitationExpenseOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationExpenseSelectedIndex { get { return IndexSelectionFromOptions(LimitationExpenseOptions, LimitationExpense); } }

        public List<string> LimitationOfContractTypeOptions { get { return new List<string>(new string[] { "Regular", "half-time", "part-time" }); } }
        public int LimitationOfContractTypeSelectedIndex { get { return IndexSelectionFromOptions(LimitationOfContractTypeOptions, LimitationOfContractType); } }

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





        private int IndexSelectionFromOptions(List<string> options, string value)
        {
            if (options != null && options.Contains(value))
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


        public Task GetContractBodyOptions(int jobId, int candidateId)
        {
            var task = GetOptions(jobId, candidateId);
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions(int jobId, int candidateId)
        {
            ContractOptions = await _api.GetContractCreationInitialPageOptions(jobId, candidateId);
        }
    }
}