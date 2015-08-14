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
		UIKit.UIView approverContainerView { get; set; }

		[Outlet]
		UIKit.UILabel approverNameLabel { get; set; }

		[Outlet]
		UIKit.UIView calendarContainerView { get; set; }

		[Outlet]
		UIKit.UILabel calendarDateLabel { get; set; }

		[Outlet]
		UIKit.UIView calendarHeaderView { get; set; }

		[Outlet]
		UIKit.UIButton calendarLeftButton { get; set; }

		[Outlet]
		UIKit.UIButton calendarRightButton { get; set; }

		[Outlet]
		UIKit.UIButton submitButton { get; set; }

		[Outlet]
		UIKit.UIView submitContainerView { get; set; }

		[Outlet]
		UIKit.UILabel submitDayYearLabel { get; set; }

		[Outlet]
		UIKit.UILabel submitMonthLabel { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }

		[Outlet]
		UIKit.UILabel totalHoursLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (calendarHeaderView != null) {
				calendarHeaderView.Dispose ();
				calendarHeaderView = null;
			}

			if (approverContainerView != null) {
				approverContainerView.Dispose ();
				approverContainerView = null;
			}

			if (addButton != null) {
				addButton.Dispose ();
				addButton = null;
			}

			if (approverNameLabel != null) {
				approverNameLabel.Dispose ();
				approverNameLabel = null;
			}

			if (calendarContainerView != null) {
				calendarContainerView.Dispose ();
				calendarContainerView = null;
			}

			if (calendarDateLabel != null) {
				calendarDateLabel.Dispose ();
				calendarDateLabel = null;
			}

			if (calendarLeftButton != null) {
				calendarLeftButton.Dispose ();
				calendarLeftButton = null;
			}

			if (calendarRightButton != null) {
				calendarRightButton.Dispose ();
				calendarRightButton = null;
			}

			if (submitButton != null) {
				submitButton.Dispose ();
				submitButton = null;
			}

			if (submitContainerView != null) {
				submitContainerView.Dispose ();
				submitContainerView = null;
			}

			if (submitDayYearLabel != null) {
				submitDayYearLabel.Dispose ();
				submitDayYearLabel = null;
			}

			if (submitMonthLabel != null) {
				submitMonthLabel.Dispose ();
				submitMonthLabel = null;
			}

			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}

			if (totalHoursLabel != null) {
				totalHoursLabel.Dispose ();
				totalHoursLabel = null;
			}
		}
	}
}
