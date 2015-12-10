using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractorJobStatusListViewModel
    {
        private IEnumerable<Contractor> _contractors;
        private readonly IMatchGuideApi _api;
		public JobStatus Status;

        public string ClientName { get; set; }

        private IEnumerable<Contractor> Contractors
        {
            get { return _contractors ?? Enumerable.Empty<Contractor>(); }
            set { _contractors = value ?? Enumerable.Empty<Contractor>(); }
        }

        public ContractorJobStatusListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

        public void LoadContractors(IEnumerable<Contractor> contractors)
        {
            Contractors = contractors ?? Enumerable.Empty<Contractor>();
        }

        public Task LoadContractorsWithJobIDAndStatusAndClientName(int Id, JobStatus status, string clientName )
        {
            ClientName = clientName;

            var task = GetContractorsWithJobIdAndStatus(Id, status);
			Status = status;

            return task;
        }

        private async Task GetContractorsWithJobIdAndStatus(int id, JobStatus status)
        {
            Contractors = await _api.GetContractorsWithJobIdAndStatus(id, status);
        }

        public int NumberOfContractors()
        {
            return Contractors.Count();
        }

        public string ContractorNameByRowNumber(int rowNumber)
        {
            var contractor = Contractors.ElementAtOrDefault(rowNumber);
            
            if (contractor == null || string.IsNullOrEmpty(contractor.ContactInformation.FullName))
                return string.Empty;

            return contractor.ContactInformation.FullName;
        }

        public string FormattedContractorStatusByRowNumber(int rowNumber)
        {
			//var contractor = _contractors.ElementAtOrDefault(rowNumber);
            //return contractor == null ? string.Empty : contractor.FullName;
			//return contractor == null ? string.Empty : contractor.;
			return string.Empty;
        }


		//Can access rates if proposed contractor
		public string FormattedBillRateByRowNumber( int rowNumber )
		{
			if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return _contractors.ElementAt(rowNumber).BillRate.ToString();

			return string.Empty;
		}

		public string FormattedPayRateByRowNumber( int rowNumber )
		{
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return _contractors.ElementAt(rowNumber).PayRate.ToString();

			return string.Empty;
		}

		public string FormattedGrossMarginByRowNumber( int rowNumber )
		{
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return _contractors.ElementAt(rowNumber).GrossMargin.ToString();

			return string.Empty;
		}

		public string FormattedMarkupByRowNumber( int rowNumber )
		{
		    if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
		        return "N/A";//_contractors.ElementAt(rowNumber).Markup.ToString();

			return string.Empty;
		}

        public int ContractorContactIdByRowNumber(int item)
        {
            return _contractors.ElementAt(item).ContactInformation.Id;
        }
    }
}
