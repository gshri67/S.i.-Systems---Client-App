using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.IO;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;

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

            LoadRemittancePDF();
            /*
			webView = new UIWebView ( View.Frame );
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.AddSubview (webView);

			string fileName = "Sprint2_SS_1.0.pdf"; // remember case-sensitive
			string localDocUrl = Path.Combine (NSBundle.MainBundle.BundlePath, fileName);
			webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
			webView.ScalesPageToFit = true;*/
		}

        public async void LoadRemittancePDF()
        {
            //IndicateLoading();

            await _viewModel.LoadPDF( string.Empty );

            UpdateWebView();
        }

	    private void UpdateWebView()
	    {
	        if (webView == null)
	        {
	            webView = new UIWebView(View.Frame);
	            webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
	            View.AddSubview(webView);
	        }

	        string fileName = "ERemittance.pdf"; // remember case-sensitive
            string localDocUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);
         
            byte[] buffer = new byte[10000];
	        int bufferSize = 10000;

            //_viewModel._PDFStream.BeginWrite(buffer, 0, bufferSize, ar => { Console.WriteLine("Wrote to file"); }, )

            FileStream outFile = new FileStream(fileName, FileMode.Create);

            int bytesRead;
            while((bytesRead = _viewModel.PDFStream.Read(buffer, 0, buffer.Length)) != 0)
                outFile.Write(buffer, 0, bytesRead);

            // Or using statement instead
	        outFile.Close();

            webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
            webView.ScalesPageToFit = true;
	    }
	}
}
