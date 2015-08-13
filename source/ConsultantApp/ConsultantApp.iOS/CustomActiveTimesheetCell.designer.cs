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

namespace ConsultantApp.iOS
{
	[Register ("CustomActiveTimesheetCell")]
	partial class CustomActiveTimesheetCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Company { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Hours { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Status { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TimesheetApprover { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Company != null) {
				Company.Dispose ();
				Company = null;
			}
			if (Hours != null) {
				Hours.Dispose ();
				Hours = null;
			}
			if (Status != null) {
				Status.Dispose ();
				Status = null;
			}
			if (TimesheetApprover != null) {
				TimesheetApprover.Dispose ();
				TimesheetApprover = null;
			}
		}
	}
}
