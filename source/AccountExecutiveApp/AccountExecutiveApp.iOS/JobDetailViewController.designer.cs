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
	[Register ("JobDetailViewController")]
	partial class JobDetailViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		AccountExecutiveApp.iOS.RightDetailCell Callouts { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		ContactCell ClientContact { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ClientContactName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		ContactCell DirectReport { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DirectReportName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		AccountExecutiveApp.iOS.RightDetailCell Proposed { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		AccountExecutiveApp.iOS.RightDetailCell ShortListed { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Callouts != null) {
				Callouts.Dispose ();
				Callouts = null;
			}
			if (ClientContact != null) {
				ClientContact.Dispose ();
				ClientContact = null;
			}
			if (ClientContactName != null) {
				ClientContactName.Dispose ();
				ClientContactName = null;
			}
			if (DirectReport != null) {
				DirectReport.Dispose ();
				DirectReport = null;
			}
			if (DirectReportName != null) {
				DirectReportName.Dispose ();
				DirectReportName = null;
			}
			if (Proposed != null) {
				Proposed.Dispose ();
				Proposed = null;
			}
			if (ShortListed != null) {
				ShortListed.Dispose ();
				ShortListed = null;
			}
		}
	}
}
