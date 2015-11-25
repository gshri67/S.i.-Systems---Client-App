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

            return task;
        }

        private async Task GetContractorsWithJobIdAndStatus(int id, JobStatus status)
        {
            if( status == JobStatus.Shortlisted )
                Contractors = await _api.GetShortlistedContractorsWithJobId( id );
            else if (status == JobStatus.Proposed)
                Contractors = await _api.GetProposedContractorsWithJobId(id);
            else if (status == JobStatus.Callout)
                Contractors = await _api.GetCalloutContractorsWithJobId(id);
        }

        public int NumberOfContractors()
        {
            return Contractors.Count();
        }

        public string ContractorNameByRowNumber(int rowNumber)
        {
            var contractor = Contractors.ElementAtOrDefault(rowNumber);
            
            if (contractor == null || string.IsNullOrEmpty(contractor.FullName))
                return string.Empty;

            return contractor.FullName;
        }

        public string ContractorStatusByRowNumber(int rowNumber)
        {
            //var contractor = Consultants.ElementAtOrDefault(rowNumber);
            //return contractor == null ? string.Empty : contractor.FullName;
            return string.Empty;
        }
    }
}
