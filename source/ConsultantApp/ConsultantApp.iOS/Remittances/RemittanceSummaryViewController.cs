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

			webView = new UIWebView ( View.Frame );
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.AddSubview (webView);

			string fileName = "Sprint2_SS_1.0.pdf"; // remember case-sensitive
			string localDocUrl = Path.Combine (NSBundle.MainBundle.BundlePath, fileName);
			webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
			webView.ScalesPageToFit = true;
		}

        public async void LoadRemittancePDF()
        {
            //IndicateLoading();

            await _viewModel.LoadPDF( string.Empty );

            int a;
        }
	}
}
