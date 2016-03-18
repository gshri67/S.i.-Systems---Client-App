using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class CurrentConsultantDetailsViewModel
	{
        private readonly IMatchGuideApi _api;

	    private ConsultantDetails _consultantDetails;

	    public Task LoadingConsultantDetails;

        public CurrentConsultantDetailsViewModel(IMatchGuideApi matchGuideApi)
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
#if TEST
            Console.WriteLine("GetConsultantDetails");
#endif
            _consultantDetails = await _api.GetCurrentUserConsultantDetails();
        }

	    private void SetCurrentConsultantDetails()
	    {
            CurrentConsultantDetails.CorporationName = ConsultantCorporationName();
	    }

	    public bool HasCorporationName()
	    {
	        return _consultantDetails != null && !string.IsNullOrEmpty(_consultantDetails.CorporationName);
	    }

        public string ConsultantCorporationName()
        {
            return HasCorporationName() 
                ? _consultantDetails.CorporationName 
                : string.Empty;
        }
	}
}

