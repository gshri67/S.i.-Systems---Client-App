// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using ConsultantApp.iOS;
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ConsultantApp.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ForgotPasswordButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton login { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView loginActivityIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView LoginView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField password { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField username { get; set; }

		[Action ("login_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void login_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ForgotPasswordButton != null) {
				ForgotPasswordButton.Dispose ();
				ForgotPasswordButton = null;
			}
			if (login != null) {
				login.Dispose ();
				login = null;
			}
			if (loginActivityIndicator != null) {
				loginActivityIndicator.Dispose ();
				loginActivityIndicator = null;
			}
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
		}
	}
}
