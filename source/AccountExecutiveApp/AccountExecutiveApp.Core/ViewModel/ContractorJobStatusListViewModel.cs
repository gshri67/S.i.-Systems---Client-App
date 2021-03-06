﻿using System;
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
        private readonly IMatchGuideApi _api;
		public JobStatus Status;
        private IEnumerable<Rate> _rateSummaries;

        public string ClientName { get; set; }
        public string _jobTitle;
        public string JobTitle
        {
            get { return _jobTitle ?? string.Empty; }
            set { _jobTitle = value ?? string.Empty; }
        }

        public int JobId { get; set; }

        private IEnumerable<Rate> RateSummaries
        {
            get { return _rateSummaries ?? Enumerable.Empty<Rate>(); }
            set { _rateSummaries = value ?? Enumerable.Empty<Rate>(); }
        }

        public ContractorJobStatusListViewModel(IMatchGuideApi api)
		{
			_api = api;
		}
/*
        public void LoadContractors(IEnumerable<Contractor> contractors)
        {
            Contractors = contractors ?? Enumerable.Empty<Contractor>();
        }
        */
        
        public Task LoadContractorsWithJobIDAndStatusAndClientName(int Id, JobStatus status, string clientName)
        {
            ClientName = clientName;

            var task = GetContractorRateSummaryWithJobIdAndStatus(Id, status);
            Status = status;

            return task;
        }

        private async Task GetContractorRateSummaryWithJobIdAndStatus(int id, JobStatus status)
        {
            RateSummaries = await _api.GetContractorRateSummaryWithJobIdAndStatus(id, status);
        }

        public int NumberOfContractors()
        {
            return RateSummaries.Count();
        }

        public string ContractorNameByRowNumber(int rowNumber)
        {
            var summary = RateSummaries.ElementAtOrDefault(rowNumber);
            
            if (summary == null || string.IsNullOrEmpty(summary.FullName))
                return string.Empty;

            return summary.FullName;
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
                return RateSummaries.ElementAt(rowNumber).BillRate.ToString();

			return string.Empty;
		}

		public string FormattedPayRateByRowNumber( int rowNumber )
		{
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return RateSummaries.ElementAt(rowNumber).PayRate.ToString();

			return string.Empty;
		}

		public string FormattedGrossMarginByRowNumber( int rowNumber )
		{
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return RateSummaries.ElementAt(rowNumber).GrossMargin.ToString();

			return string.Empty;
		}

		public string FormattedMarkupByRowNumber( int rowNumber )
		{
		    if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
				return string.Format("{0:P2}", RateSummaries.ElementAt(rowNumber).Markup );

			return string.Empty;
		}

        public string FormattedMarginByRowNumber(int rowNumber)
        {
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
				return string.Format("{0:P2}", RateSummaries.ElementAt(rowNumber).Margin );

            return string.Empty;
        }

        public int ContractorContactIdByRowNumber(int rowNumber)
        {
            return RateSummaries.ElementAt(rowNumber).ContractorId;
        }

        public string FormattedDateByRowNumber(int rowNumber)
        {
            if (Status == JobStatus.Proposed || Status == JobStatus.Callout)
                return RateSummaries.ElementAt(rowNumber).Date.ToString("MMM d yyyy");
            
            return string.Empty;
        }
    }
}
