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
	[Register ("ActiveTimesheetViewController")]
	partial class ActiveTimesheetViewController
	{
		[Outlet]
		UIKit.UITableView tableview { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ActiveTimesheets { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView ActiveTimesheetsTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ActiveTimesheets != null) {
				ActiveTimesheets.Dispose ();
				ActiveTimesheets = null;
			}
			if (ActiveTimesheetsTable != null) {
				ActiveTimesheetsTable.Dispose ();
				ActiveTimesheetsTable = null;
			}
		}
	}
}
