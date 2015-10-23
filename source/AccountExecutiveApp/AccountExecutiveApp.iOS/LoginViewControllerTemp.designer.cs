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
	[Register ("LoginViewControllerTemp")]
	partial class LoginViewControllerTemp
	{
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
