// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace App2
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIButton forgotPasswordButton { get; set; }

		[Outlet]
		UIKit.UIButton login { get; set; }

		[Outlet]
		UIKit.UITextField password { get; set; }

		[Outlet]
		App2.BorderedButton signUpButton { get; set; }

		[Outlet]
		UIKit.UITextField username { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (login != null) {
				login.Dispose ();
				login = null;
			}

			if (username != null) {
				username.Dispose ();
				username = null;
			}

			if (password != null) {
				password.Dispose ();
				password = null;
			}

			if (forgotPasswordButton != null) {
				forgotPasswordButton.Dispose ();
				forgotPasswordButton = null;
			}

			if (signUpButton != null) {
				signUpButton.Dispose ();
				signUpButton = null;
			}
		}
	}
}
