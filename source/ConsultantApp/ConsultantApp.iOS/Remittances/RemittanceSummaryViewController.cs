using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.IO;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using System.Net.Http;
using System.Net;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
	partial class RemittanceSummaryViewController : UIViewController
	{
		private UIWebView webView;
	    
        private readonly RemittanceSummaryViewModel _viewModel;

		public RemittanceSummaryViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<RemittanceSummaryViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            /*
			webView = new UIWebView ( View.Frame );
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.AddSubview (webView);

			string fileName = "Sprint2_SS_1.0.pdf"; // remember case-sensitive
			string localDocUrl = Path.Combine (NSBundle.MainBundle.BundlePath, fileName);
			webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
			webView.ScalesPageToFit = true;*/
		}

	    public void SetRemittance( Remittance remittance )
	    {
            LoadRemittancePDF(remittance);
	    }

	    public async void LoadRemittancePDF( Remittance remittance )
        {
            //IndicateLoading();
            await _viewModel.LoadPDF(remittance);

            UpdateWebView();
        }

	    private async void UpdateWebView()
	    {
	        if (webView == null)
	        {
	            webView = new UIWebView(View.Frame);
	            webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
	            View.AddSubview(webView);
	        }

	        string fileName = "ERemittance.pdf"; // remember case-sensitive
            string localDocUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);


            int bufferSize = (int)_viewModel.PDFStream.Length;
            byte[] buffer = new byte[bufferSize];

            FileStream outFile = new FileStream(fileName, FileMode.Create);

            int bytesRead;
            while((bytesRead = _viewModel.PDFStream.Read(buffer, 0, bufferSize)) != 0)
                outFile.Write(buffer, 0, bytesRead);
            
  
	        outFile.Close();

            webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
            webView.ScalesPageToFit = true;
	    }
	}
}
