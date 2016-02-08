// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton login { get; set; }

		[Action ("login_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void login_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (login != null) {
				login.Dispose ();
				login = null;
			}
		}
	}
}
