using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ClientApp.iOS
{
	partial class ResumeViewController : UIViewController
	{
        public string Resume { get; set; }

        public ResumeViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

	        ResumeText.Text = Resume;
	    }
	}
}
