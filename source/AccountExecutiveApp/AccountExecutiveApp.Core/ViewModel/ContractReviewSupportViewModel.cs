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