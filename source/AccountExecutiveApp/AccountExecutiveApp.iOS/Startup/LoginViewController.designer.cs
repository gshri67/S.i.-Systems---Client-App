// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AccountExecutiveApp.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView loginActivityIndicator { get; set; }

		[Outlet]
		UIKit.UIView LoginView { get; set; }

		[Outlet]
		UIKit.UITextField password { get; set; }

		[Outlet]
		UIKit.UITextField username { get; set; }

		[Action ("login_TouchUpInside:")]
		partial void login_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LoginView != null) {
				LoginView.Dispose ();
				LoginView = null;
			}

			if (password != null) {
				password.Dispose ();
				password = null;
			}

			if (username != null) {
				username.Dispose ();
				username = null;
			}

			if (loginActivityIndicator != null) {
				loginActivityIndicator.Dispose ();
				loginActivityIndicator = null;
			}
		}
	}
}
