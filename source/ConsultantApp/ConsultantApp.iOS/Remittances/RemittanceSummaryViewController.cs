using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.IO;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using System.Net.Http;
using System.Net;
using System.Text;
using CoreGraphics;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
	partial class RemittanceSummaryViewController : UIViewController
	{
		private UIWebView webView;
	    
        private readonly RemittanceSummaryViewModel _viewModel;
        private LoadingOverlay _overlay;

		public RemittanceSummaryViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<RemittanceSummaryViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		    Title = "ERemittance Summary";
		}

	    public void SetRemittance( Remittance remittance )
	    {
            LoadRemittancePDF(remittance);
	    }

	    public async void LoadRemittancePDF( Remittance remittance )
        {
            IndicateLoading();
            await _viewModel.LoadPDF(remittance);

            UpdateWebView();
        }

	    private void UpdateWebView()
	    {
            RemoveOverlay();

	        if (webView == null)
	        {
	            webView = new UIWebView(View.Frame);
	            webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
	            View.AddSubview(webView);
	        }

            try
            {
	            string fileName = "ERemittance.pdf"; // remember case-sensitive
                string localDocUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);
                /*
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                localDocUrl = Path.Combine(documents, fileName);

                var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var libPath = Path.Combine(docsPath, "..", "Library");
                localDocUrl = Path.Combine(libPath, fileName);
                

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string localDocUrl = Path.Combine(documents, fileName);
                */

                //localDocUrl = Path.Combine(NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0].Path, fileName);

                //int bufferSize = (int)_viewModel.PDFStream.Length;
                //byte[] buffer = new byte[bufferSize];

                FileStream outFile = new FileStream(localDocUrl, FileMode.Create);
                _viewModel.PDFStream.CopyTo(outFile);
  
	            outFile.Close();

                //NSData streamData = NSData.FromStream(_viewModel.PDFStream);
                //webView.LoadData(streamData, "application/pdf", Encoding.UTF8.ToString(), null);
               
                
	            webView.LoadRequest(new NSUrlRequest(new NSUrl(localDocUrl, false)));
	            webView.ScalesPageToFit = true;
	        }
            catch (Exception e)
            {
                var exceptionMessage = e.Message;
            }
            
	    }

    #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);
                _overlay = new LoadingOverlay(frame, null);
                View.Add(_overlay);
            });
        }

        private void RemoveOverlay()
        {
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
        }

    #endregion
    }
}
