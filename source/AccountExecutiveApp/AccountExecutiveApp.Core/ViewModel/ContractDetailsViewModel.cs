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
        private bool _successfullyLoadedContracts {
            get
            {
                if (Contract.ClientName == string.Empty)
                    return false;
                return true;
            }
        }
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
                if (!_successfullyLoadedContracts)
                    return string.Empty;

				return string.Format("{0} - {1}", Contract.StartDate.ToString("MMM dd, yyyy"),
                    Contract.EndDate.ToString("MMM dd, yyyy"));
            }
        }
		public string FormattedStartDate
		{
		    get
		    {
                if (!_successfullyLoadedContracts)
                    return string.Empty;

		        return string.Format("{0}", Contract.StartDate.ToString("MMM dd, yyyy"));
		    }
		}
		public string FormattedEndDate
		{
		    get
		    {
                if (!_successfullyLoadedContracts)
                    return string.Empty;

		        return string.Format("{0}", Contract.EndDate.ToString("MMM dd, yyyy"));
		    }
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
            //get { return string.Format("{0:C}", Contract.Markup); }
            get { return string.Format("N/A"); }
		}

		public string FormattedClientAndStatus
		{
		    get
		    {
                if (!_successfullyLoadedContracts)
                    return string.Empty;

		        return string.Format("{0} | {1}", Contract.ClientName, Contract.AgreementSubType.ToString());
		    }
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
                return Contract.Contractor == null || Contract.Contractor.ContactInformation == null
                    ? String.Empty
                    : Contract.Contractor.ContactInformation.FullName;
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
