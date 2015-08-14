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

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	[Register ("ActiveTimesheetCell")]
	partial class ActiveTimesheetCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel companyLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel hoursLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel statusLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel timesheetApproverLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (companyLabel != null) {
				companyLabel.Dispose ();
				companyLabel = null;
			}
			if (hoursLabel != null) {
				hoursLabel.Dispose ();
				hoursLabel = null;
			}
			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}
			if (timesheetApproverLabel != null) {
				timesheetApproverLabel.Dispose ();
				timesheetApproverLabel = null;
			}
		}
	}
}
