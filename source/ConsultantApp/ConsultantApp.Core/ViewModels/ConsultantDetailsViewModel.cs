using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class ConsultantDetailsViewModel
	{
        private readonly IMatchGuideApi _api;

	    private ConsultantDetails _consultantDetails;

	    public Task LoadingConsultantDetails;

        public ConsultantDetailsViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;

            _consultantDetails = new ConsultantDetails();
        }

	    public void LoadConsultantDetails()
	    {
            LoadingConsultantDetails = GetConsultantDetails();
            LoadingConsultantDetails.ContinueWith(_ => SetCurrentConsultantDetails());
	    }

        private async Task GetConsultantDetails()
        {
            _consultantDetails = await _api.GetCurrentUserConsultantDetails();
        }

	    private void SetCurrentConsultantDetails()
	    {
            CurrentConsultantDetails.CorporationName = ConsultantCorporationName();
	    }

        public string ConsultantCorporationName()
        {
            if (_consultantDetails == null || string.IsNullOrEmpty(_consultantDetails.CorporationName))
                return string.Empty;

            return _consultantDetails.CorporationName;
        }
	}
}

