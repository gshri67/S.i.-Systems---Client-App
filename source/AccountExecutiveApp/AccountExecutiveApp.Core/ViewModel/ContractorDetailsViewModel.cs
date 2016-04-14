using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractorDetailsViewModel
    {
        private readonly IMatchGuideApi _api;

        private Contractor _contractor;
        public Contractor Contractor
        {
            get { return _contractor ?? new Contractor(); }
            set { _contractor = value ?? new Contractor(); }
        }

        public ContractorDetailsViewModel(IMatchGuideApi api)
		{
			_api = api;
		}

        public string PageTitle {
            get {
                return Contractor != null ? Contractor.ContactInformation.FullName : string.Empty;
            }
        }

        public Task LoadContractor( int Id )
        {
            var task = GetContractor( Id );

            return task;
        }

        private async Task GetContractor( int Id )
        {
            Contractor = await _api.GetContractorById(Id);
        }

        public int ContractIdFromJobIdAndContractorId(int jobId, int contractorId)
        {
            return 0;
            //return GetContractIdFromJobIdAndContractorId(jobId, contractorId).Result;
        }
        public async Task<int> GetContractIdFromJobIdAndContractorId(int jobId, int contractorId)
        {
            return await _api.GetContractIdFromJobIdAndContractorId(jobId, contractorId);
        }
    }
}