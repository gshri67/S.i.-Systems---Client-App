using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class SearchViewModel
    {
        private readonly IMatchGuideApi _api;

        private IEnumerable<Contractor> _contractors;

        public IEnumerable<Contractor> Contractors
        {
            get { return _contractors ?? Enumerable.Empty<Contractor>(); }
            set { _contractors = value ?? Enumerable.Empty<Contractor>(); }
        }

        public SearchViewModel(IMatchGuideApi api)
		{
			_api = api;
		}
        
        public Task LoadContractors()
        {
            var task = GetContractors();

            return task;
        }

        private async Task GetContractors()
        {
            Contractors = await _api.GetContractors();
        }



        public Task LoadClientContacts()
        {
            var task = GetClientContacts();

            return task;
        }

        private async Task GetClientContacts()
        {
            Contractors = await _api.GetContractors();
        }
    }
}