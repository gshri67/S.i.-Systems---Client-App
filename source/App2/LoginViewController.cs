using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class LoginViewController : UIViewController
	{
		public LoginViewController(){
		}

		public LoginViewController (IntPtr handle) : base (handle)
		{
			
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
		
			NavigationController.NavigationBar.Hidden = true;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			login.TouchUpInside += delegate {
				NavigationController.NavigationBar.Hidden = false;
				NavigationController.PopViewController(true);
			};
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
