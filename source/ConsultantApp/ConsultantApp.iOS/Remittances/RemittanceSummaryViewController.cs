using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.IO;

namespace ConsultantApp.iOS
{
	partial class RemittanceSummaryViewController : UIViewController
	{
		private UIWebView webView;

		public RemittanceSummaryViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			webView = new UIWebView ( View.Frame );
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.AddSubview (webView);

			string fileName = "Sprint2_SS_1.0.pdf"; // remember case-sensitive
			string localDocUrl = Path.Combine (NSBundle.MainBundle.BundlePath, fileName);
			webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
			webView.ScalesPageToFit = true;
		}
	}
}
