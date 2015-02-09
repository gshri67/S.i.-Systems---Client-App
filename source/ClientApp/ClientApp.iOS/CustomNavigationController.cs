using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using UIKit;

namespace ClientApp.iOS
{
	partial class CustomNavigationController : UINavigationController
	{
		public CustomNavigationController (IntPtr handle) : base (handle)
		{
		}

	    private void AdjustNavBar()
	    {
            this.NavigationBar.BarTintColor = UIColor.White;
            this.NavigationBar.TintColor = UIColor.Gray;

            this.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.FromName("TimesNewRomanPSMT", 20),
                StrokeColor = UIColor.FromRGB(91, 91, 92)//5b5b5c in styleguide
            };
	    }

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

            AdjustNavBar();
	    }
	}
}
