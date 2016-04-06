using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractReviewSupportViewModel
    {
        private readonly IMatchGuideApi _api;

        private ContractCreationOptions _contractOptions;
        private ContractCreationOptions ContractOptions
        {
            get { return _contractOptions ?? new ContractCreationOptions(); }
            set { _contractOptions = value ?? new ContractCreationOptions(); }
        }

        public ContractReviewSupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

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