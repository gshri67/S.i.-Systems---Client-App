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
		UIKit.UIButton login { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (login != null) {
				login.Dispose ();
				login = null;
			}
		}
	}
}