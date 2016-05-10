using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractReviewSupportViewModel
    {
        private readonly IMatchGuideApi _api;

        private ContractCreationDetails_Review _contractDetails;
        private ContractCreationDetails_Review ContractDetails
        {
            get { return _contractDetails ?? new ContractCreationDetails_Review(); }
            set { _contractDetails = value ?? new ContractCreationDetails_Review(); }
        }

        public ContractReviewSupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        public List<string> BooleanOptions { get { return new List<string>(new string[] { "Yes", "No" }); } }

        public int BooleanOptionIndex(bool value)
        {
            if (value == true)
                return 0;
            return 1;
        }

        public bool WebTimesheetAccess 
        {
            get { return ContractDetails.WebTimeSheetAccess; }
        }
        public bool WebTimesheetProjectAccess
        {
            get { return ContractDetails.WebTimeSheetProjectAccess; }
        }
        public string TimesheetType
        {
            get { return ContractDetails.TimesheetType ?? string.Empty; }
        }
        public string Vertical
        {
            get { return ContractDetails.Vertical ?? string.Empty; }
        }
        /*
        public string InvoiceFormat
        {
            get { return ContractDetails.InvoiceFormat ?? string.Empty; }
        }*/
        public IEnumerable<string> InvoiceInformation
        {
            get { return ContractDetails.InvoiceInformation ?? Enumerable.Empty<string>(); }
        }
        public IEnumerable<string> AssociatedProjectsAndPOs
        {
            get { return ContractDetails.AssociatedProjectAndPOs ?? Enumerable.Empty<string>(); }
        }

        public string FormattedWebTimesheetAccess()
        {
            return BooleanOptions[ BooleanOptionIndex(WebTimesheetAccess) ];
        }
        public string FormattedWebTimesheetProjectAccess()
        {
            return BooleanOptions[BooleanOptionIndex(WebTimesheetProjectAccess)];
        }

        public Task GetContractReviewDetails( int jobId, int candidateId )
        {
            var task = GetDetails( jobId, candidateId );
            //todo: task.continueWith
            return task;
        }

        private async Task GetDetails( int jobId, int candidateId )
        {
            ContractDetails = await _api.GetDetailsForReviewContractCreationForm( jobId, candidateId );
        }
    }
}