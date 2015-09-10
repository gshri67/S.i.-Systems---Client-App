// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ConsultantApp.iOS
{
	[Register ("AddTimeViewController")]
	partial class AddTimeViewController
	{
		[Outlet]
		UIKit.UIButton addButton { get; set; }

		[Outlet]
		UIKit.UIButton copyOverButton { get; set; }

		[Outlet]
		UIKit.UIView headerContainer { get; set; }

		[Outlet]
		UIKit.UILabel headerDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel headerHoursLabel { get; set; }

		[Outlet]
		UIKit.UIView headerLabelsContainer { get; set; }

		[Outlet]
		UIKit.UIButton leftArrowButton { get; set; }

		[Outlet]
		UIKit.UIButton rightArrowButton { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addButton != null) {
				addButton.Dispose ();
				addButton = null;
			}

			if (headerContainer != null) {
				headerContainer.Dispose ();
				headerContainer = null;
			}

			if (headerDateLabel != null) {
				headerDateLabel.Dispose ();
				headerDateLabel = null;
			}

			if (headerHoursLabel != null) {
				headerHoursLabel.Dispose ();
				headerHoursLabel = null;
			}

			if (headerLabelsContainer != null) {
				headerLabelsContainer.Dispose ();
				headerLabelsContainer = null;
			}

			if (leftArrowButton != null) {
				leftArrowButton.Dispose ();
				leftArrowButton = null;
			}

			if (rightArrowButton != null) {
				rightArrowButton.Dispose ();
				rightArrowButton = null;
			}

			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}

			if (copyOverButton != null) {
				copyOverButton.Dispose ();
				copyOverButton = null;
			}
		}
	}
}
