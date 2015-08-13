// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	[Register ("TimesheetOverviewViewController")]
	partial class TimesheetOverviewViewController
	{
		[Outlet]
		UIKit.UIButton addButton { get; set; }

		[Outlet]
		UIKit.UIView calendarContainerView { get; set; }

		[Outlet]
		UIKit.UIButton submitButton { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addButton != null) {
				addButton.Dispose ();
				addButton = null;
			}

			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}

			if (calendarContainerView != null) {
				calendarContainerView.Dispose ();
				calendarContainerView = null;
			}

			if (submitButton != null) {
				submitButton.Dispose ();
				submitButton = null;
			}
		}
	}
}
