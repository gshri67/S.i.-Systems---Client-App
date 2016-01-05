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

        private IEnumerable<UserContact> _clientContacts;

        public IEnumerable<UserContact> ClientContacts
        {
            get { return _clientContacts ?? Enumerable.Empty<UserContact>(); }
            set { _clientContacts = value ?? Enumerable.Empty<UserContact>(); }
        }

        public SearchViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

        public Task LoadSearchData()
        {
            var task = GetSearchData();

            return task;
        }

        private async Task GetSearchData()
        {
            Contractors = await _api.GetContractors();
            ClientContacts = await _api.GetClientContacts();
        }
    }
}