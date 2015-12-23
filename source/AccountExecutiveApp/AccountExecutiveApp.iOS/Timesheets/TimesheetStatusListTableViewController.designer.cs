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
	[Register ("TimesheetStatusListTableViewController")]
	partial class TimesheetStatusListTableViewController
	{
		[Outlet]
		AccountExecutiveApp.iOS.RightDetailCell CancelledTimesheetsCell { get; set; }

		[Outlet]
		AccountExecutiveApp.iOS.RightDetailCell OpenTimesheetsCell { get; set; }

		[Outlet]
		AccountExecutiveApp.iOS.RightDetailCell RejectedTimesheetsCell { get; set; }

		[Outlet]
		AccountExecutiveApp.iOS.RightDetailCell SubmittedTimesheetsCell { get; set; }

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
