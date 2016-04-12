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

        public bool WebTimesheetAccess 
        {
            get { return _contractDetails.WebTimeSheetAccess; }
        }
        public bool WebTimesheetProjectAccess
        {
            get { return _contractDetails.WebTimeSheetProjectAccess; }
        }
        public string TimesheetType
        {
            get { return _contractDetails.TimesheetType ?? string.Empty; }
        }
        public string Vertical
        {
            get { return _contractDetails.Vertical ?? string.Empty; }
        }
        public string InvoiceFormat
        {
            get { return _contractDetails.InvoiceFormat ?? string.Empty; }
        }
        public IEnumerable<string> InvoiceInformation
        {
            get { return _contractDetails.InvoiceInformation ?? Enumerable.Empty<string>(); }
        }
        public IEnumerable<string> AssociatedProjectsAndPOs
        {
            get { return _contractDetails.AssociatedProjectAndPOs ?? Enumerable.Empty<string>(); }
        }

        public Task GetContractReviewDetails()
        {
            var task = GetDetails();
            //todo: task.continueWith
            return task;
        }

        private async Task GetDetails()
        {
           // ContractOptions = await _api.GetDropDownValuesForInitialContractCreationForm();
        }
    }
}