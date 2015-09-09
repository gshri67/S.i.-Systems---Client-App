// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	[Register ("ActiveTimesheetViewController")]
	partial class ActiveTimesheetViewController
	{
		[Outlet]
		UIKit.UIView ActiveTimesheets { get; set; }

		[Outlet]
		UIKit.UITableView ActiveTimesheetsTable { get; set; }

		[Outlet]
		UIKit.UILabel noTimesheetsLabel { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}

			if (ActiveTimesheets != null) {
				ActiveTimesheets.Dispose ();
				ActiveTimesheets = null;
			}

			if (ActiveTimesheetsTable != null) {
				ActiveTimesheetsTable.Dispose ();
				ActiveTimesheetsTable = null;
			}

			if (noTimesheetsLabel != null) {
				noTimesheetsLabel.Dispose ();
				noTimesheetsLabel = null;
			}
		}
	}
}
