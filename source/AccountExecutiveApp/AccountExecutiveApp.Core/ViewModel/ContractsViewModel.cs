using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractsViewModel
    {
        private readonly IMatchGuideApi _api;

        private IEnumerable<ConsultantContractSummary> _contracts;
        public IEnumerable<ConsultantContractSummary> Contracts { get { return _contracts ?? Enumerable.Empty<ConsultantContractSummary>(); } }

        public ContractsViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public Task LoadContracts()
        {
            var task = GetContracts();
            //todo: task.ContinueWith if necessary
            
            return task;
        }

        public async Task GetContracts()
        {
            _contracts = await this._api.GetContracts();
        }

        public void SetContracts(IEnumerable<ConsultantContractSummary> contracts)
        {
            _contracts = contracts;
        }
    }
}
