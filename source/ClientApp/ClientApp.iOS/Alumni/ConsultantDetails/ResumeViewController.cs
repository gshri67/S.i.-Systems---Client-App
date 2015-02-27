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

	        if (Resume != null)
	        {
                ResumeView.LoadHtmlString(ConvertToHtmlString(Resume), null);
	        }
	    }

	    private string ConvertToHtmlString(string resume)
	    {
	        var replacedResume = resume.Replace("\n\r", "<br>").Replace("\n", "<br>");
            return string.Format(@"<html>
                <head>
                    <style type='text/css'>
                      body {{
                        font-family: 'HelveticaNeue-Light', 'Helvetica Neue Light', 'Helvetica Neue';
                      }}
                    </style>
                </head>
                <body>
                  {0}
                </body>
                </html>", replacedResume);
	    }
	}
}
