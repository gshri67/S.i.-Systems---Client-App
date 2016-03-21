using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using System.Net.Http;

namespace ConsultantApp.Core.ViewModels
{
	public class RemittanceViewModel
	{
		private readonly IMatchGuideApi _api;

		public RemittanceViewModel(IMatchGuideApi matchGuideApi)
		{
			_api = matchGuideApi;
		}

		public Task<IEnumerable<Remittance>> GetRemittances()
		{
			return _api.GetRemittances();
		}
        /*
        public Task< HttpResponseMessage > GetPDF( string docNumber )
        {
            return _api.GetPDF( docNumber );
        }*/
	}
}


