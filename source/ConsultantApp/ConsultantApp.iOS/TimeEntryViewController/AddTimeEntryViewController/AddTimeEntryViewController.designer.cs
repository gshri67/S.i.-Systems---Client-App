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

namespace ConsultantApp.iOS.TimeEntryViewController.AddTimeEntryViewController
{
	[Register ("AddTimeEntryViewController")]
	partial class AddTimeEntryViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField clientTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField hoursTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField projectCodeTextField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (clientTextField != null) {
				clientTextField.Dispose ();
				clientTextField = null;
			}
			if (hoursTextField != null) {
				hoursTextField.Dispose ();
				hoursTextField = null;
			}
			if (projectCodeTextField != null) {
				projectCodeTextField.Dispose ();
				projectCodeTextField = null;
			}
		}
	}
}
