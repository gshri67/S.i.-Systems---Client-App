using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractDetailsViewModel
    {
        private readonly IMatchGuideApi _api;

        private ConsultantContract _contract;
        public ConsultantContract Contract 
        {
            get { return _contract ?? new ConsultantContract(); }
            set { _contract = value ?? new ConsultantContract(); } 
        }

        public ContractDetailsViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public string CompanyName
        {
            get { return Contract.ClientName ?? string.Empty; }
        }

        public string ContractPeriod
        {
            get
            {
				return string.Format("{0} - {2}", Contract.StartDate.ToString("MMM dd, yyyy"),
                    Contract.EndDate.ToString("MMM dd, yyyy"));
            }
        }
		public string FormattedStartDate
		{
			get { return string.Format("{0}", Contract.StartDate.ToString("MMM dd, yyyy")); }
		}
		public string FormattedEndDate
		{
			get { return string.Format("{0}", Contract.EndDate.ToString("MMM dd, yyyy")); }
		}

        public string FormattedBillRate
        {
            get { return string.Format("{0:C}", Contract.BillRate); }
        }

        public string FormattedPayRate
        {
            get { return string.Format("{0:C}", Contract.PayRate); }
        }

        public string FormattedGrossMargin
        {
            get { return string.Format("{0:C}", Contract.GrossMargin); }
        }

		public string FormattedMarkup
		{
			get { return string.Format("{0:C}", Contract.Markup); }
		}

		public string FormattedClientAndStatus
		{
			get { return string.Format("{0} | {1}", Contract.ClientName, Contract.AgreementSubType.ToString()); }
		}

        public string ContractTitle
        {
            get
            {
                return Contract.Title ?? string.Empty;
            }
        }

        public string ConsultantsFullName
        {
            get
            {
                return Contract.Contractor == null 
                    ? String.Empty 
                    : Contract.Contractor.FullName;
            }
        }

        public Task LoadContractDetails(int contractId)
        {
            var task = GetContractDetailsFromService(contractId);

            return task;
        }

        private async Task GetContractDetailsFromService(int contractId)
        {
            Contract = await _api.GetContractDetailsById(contractId);
        }

    }
}
