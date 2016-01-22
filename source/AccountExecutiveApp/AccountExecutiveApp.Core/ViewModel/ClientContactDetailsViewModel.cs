using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ClientContactDetailsViewModel
    {
        private readonly IMatchGuideApi _api;

        private UserContactType ContactType { get; set; }

        private UserContact _contact;
        public UserContact Contact
        {
            get { return _contact ?? new UserContact(); }
            set { _contact = value ?? new UserContact(); }
        }

        public string LinkedInString { get { return Contact.LinkedInUrl; } }

        public ClientContactDetailsViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        public string PageTitle
        {
            get { return Contact.FullName ?? string.Empty; }
        }

        public string PageSubtitle
        {
            get
            {
                switch (ContactType)
                {
                    case UserContactType.ClientContact:
                        return "Client Contact";
                    case UserContactType.DirectReport:
                        return "Direct Report";
                    case UserContactType.BillingContact:
                        return "Billing Contact";
                }
                return string.Empty;
            }
        }

        public Task LoadContact(int Id)
        {
            var task = GetContact(Id);

            return task;
        }

        private async Task GetContact(int id)
        {
            Contact = await _api.GetUserContactById(id);
        }

		public string ClientName
		{
			get { return Contact.ClientName ?? string.Empty; }
		}

		public string Address
		{
            get { return Contact.Address ?? string.Empty; }
		}

        public void SetContactType(UserContactType contactType)
        {
            ContactType = contactType;
        }


    }
}