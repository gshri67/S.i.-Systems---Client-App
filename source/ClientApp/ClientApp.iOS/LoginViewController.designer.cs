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

namespace ClientApp.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton login { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView loginActivityIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField password { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton resetPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField username { get; set; }

		[Action ("login_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void login_TouchUpInside (UIButton sender);

		[Action ("resetPassword_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void resetPassword_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (login != null) {
				login.Dispose ();
				login = null;
			}
			if (loginActivityIndicator != null) {
				loginActivityIndicator.Dispose ();
				loginActivityIndicator = null;
			}
			if (password != null) {
				password.Dispose ();
				password = null;
			}
			if (resetPassword != null) {
				resetPassword.Dispose ();
				resetPassword = null;
			}
			if (username != null) {
				username.Dispose ();
				username = null;
			}
		}
	}
}