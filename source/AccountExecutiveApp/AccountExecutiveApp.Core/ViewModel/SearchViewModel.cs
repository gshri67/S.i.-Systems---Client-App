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

        private IEnumerable<UserContact> _contractors;

        public IEnumerable<UserContact> Contractors
        {
            get { return _contractors ?? Enumerable.Empty<UserContact>(); }
            set { _contractors = value ?? Enumerable.Empty<UserContact>(); }
        }

        private IEnumerable<UserContact> _clientContacts;

        public IEnumerable<UserContact> ClientContacts
        {
            get { return _clientContacts ?? Enumerable.Empty<UserContact>(); }
            set { _clientContacts = value ?? Enumerable.Empty<UserContact>(); }
        }

        private IEnumerable<UserContact> _filteredContractors;

        public IEnumerable<UserContact> FilteredContractors
        {
            get { return _contractors ?? Enumerable.Empty<UserContact>(); }
            set { _contractors = value ?? Enumerable.Empty<UserContact>(); }
        }

        private IEnumerable<UserContact> _filteredClientContacts;

        public IEnumerable<UserContact> FilteredClientContacts
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
            //Contractors = await _api.GetContractors();
            ClientContacts = await _api.GetClientContacts();
        }

        public Task LoadSearchDataWithFilter( string filter )
        {
            var task = GetSearchDataWithFilter( filter );

            return task;
        }

        private async Task GetSearchDataWithFilter( string filter )
        {
            IEnumerable<UserContact> FilteredContacts = await _api.GetClientContactsWithFilter( filter );

            if (FilteredContacts == null || FilteredContacts.Count() == 0)
            {
                FilteredClientContacts = Enumerable.Empty<UserContact>();
                FilteredContractors = Enumerable.Empty<UserContact>();
            }
            else
            {
                FilteredClientContacts =
                    FilteredContacts.Where(contact => !string.IsNullOrEmpty(contact.ClientName))
                        .OrderBy(contact => contact.FullName);
                FilteredContractors =
                    FilteredContacts.Where(contact => string.IsNullOrEmpty(contact.ClientName))
                        .OrderBy(contact => contact.FullName);
            }
        }
    }
}