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

        public string CompanyNameString()
        {
            if (_contract != null)
                return _contract.CompanyName;
            return string.Empty;
        }

        public string DatePeriodString()
        {
            if (_contract != null)
                return string.Format("{0} - {1}", _contract.StartDate.ToString("MMM dd, yyyy"), _contract.EndDate.ToString("MMM dd, yyyy"));
            return string.Empty;
        }

        public string BillRateString()
        {
            if (_contract != null)
                return string.Format("${0}", _contract.BillRate.ToString("0.00"));
            return string.Empty;
        }

        public string PayRateString()
        {
            if (_contract != null)
                return string.Format("${0}", _contract.PayRate.ToString("0.00"));
            return string.Empty;
        }

        public string GrossMarginString()
        {
            if (_contract != null)
                return string.Format("${0}", _contract.GrossMargin.ToString("0.00"));
            return string.Empty;
        }
        public string CompanyName
        {
            get { return Contract.CompanyName ?? string.Empty; }
        }

        public string ContractPeriod
        {
            get
            {
                return string.Format("{0} - {1}", Contract.StartDate.ToString("MMM dd, yyyy"),
                    Contract.EndDate.ToString("MMM dd, yyyy"));
            }
        }

        public string BillRate
        {
            get { return string.Format("{0:C}", Contract.BillRate); }
        }

        public string PayRate
        {
            get { return string.Format("{0:C}", Contract.PayRate); }
        }

        public string GrossMargin
        {
            get { return string.Format("{0:C}", Contract.GrossMargin); }
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
                return Contract.consultant == null 
                    ? String.Empty 
                    : Contract.consultant.FullName;
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
