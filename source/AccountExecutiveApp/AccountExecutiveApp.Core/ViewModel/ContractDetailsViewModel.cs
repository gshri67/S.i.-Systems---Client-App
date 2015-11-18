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
        public ConsultantContract Contract { get { return _contract; } }

        public ContractDetailsViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task<ConsultantContract> GetContractWithId(int id)
        {
            _contract = await _api.GetContractWithId(id);
            return _contract;
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
    }
}
