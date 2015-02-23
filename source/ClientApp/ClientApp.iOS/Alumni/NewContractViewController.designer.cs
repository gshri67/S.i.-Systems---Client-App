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
	[Register ("NewContractViewController")]
	partial class NewContractViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField RateField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ServiceLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TotalLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
			if (RateField != null) {
				RateField.Dispose ();
				RateField = null;
			}
			if (ServiceLabel != null) {
				ServiceLabel.Dispose ();
				ServiceLabel = null;
			}
			if (TotalLabel != null) {
				TotalLabel.Dispose ();
				TotalLabel = null;
			}
		}
	}
}
