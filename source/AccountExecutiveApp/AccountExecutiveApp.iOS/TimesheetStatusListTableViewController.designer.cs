// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

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
			if (SubmittedTimesheetsCell != null) {
				SubmittedTimesheetsCell.Dispose ();
				SubmittedTimesheetsCell = null;
			}

			if (RejectedTimesheetsCell != null) {
				RejectedTimesheetsCell.Dispose ();
				RejectedTimesheetsCell = null;
			}

			if (CancelledTimesheetsCell != null) {
				CancelledTimesheetsCell.Dispose ();
				CancelledTimesheetsCell = null;
			}

			if (OpenTimesheetsCell != null) {
				OpenTimesheetsCell.Dispose ();
				OpenTimesheetsCell = null;
			}
		}
	}
}
