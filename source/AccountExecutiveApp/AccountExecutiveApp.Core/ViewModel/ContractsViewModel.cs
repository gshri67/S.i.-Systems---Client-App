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

        public ContractsViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task<IEnumerable<ConsultantContract>> getContracts()
        {
            return await this._api.getContracts();
        }
    }
}
