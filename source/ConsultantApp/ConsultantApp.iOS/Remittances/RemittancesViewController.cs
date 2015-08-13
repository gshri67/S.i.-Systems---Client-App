using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ConsultantApp.iOS
{
	partial class RemittancesViewController : UIViewController
	{
		public RemittancesViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			LogoutManager.CreateNavBarLeftButton(this);
		}
	}
}
