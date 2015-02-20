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

            //TODO replace this with a proper function with more rules that we should replace
	        if (Resume != null)
	        {
                ResumeView.LoadHtmlString(Resume.Replace("\n\r", "<br>").Replace("\n", "<br>"), null);
	        }
	    }
	}
}
