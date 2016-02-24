using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using System.Net.Http;

namespace ConsultantApp.Core.ViewModels
{
    public class RemittanceSummaryViewModel
	{
		private readonly IMatchGuideApi _api;
        
        private Stream _PDFStream;
        public Stream PDFStream
        {
            get { return _PDFStream ?? new MemoryStream(); }
            set { _PDFStream = value ?? new MemoryStream(); }
        }

		public RemittanceSummaryViewModel(IMatchGuideApi matchGuideApi)
		{
			_api = matchGuideApi;
		} 

        public Task LoadPDF(string docNumber)
        {
            var task = GetPDFFromService(docNumber);

            return task;    
        }

        private async Task GetPDFFromService(string docNumber)
        {
            PDFStream = await _api.GetPDF(docNumber);
        }
	}
}


