// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace App2
{
	[Register ("DayTimeSheetViewController")]
	partial class DayTimeSheetViewController
	{
		[Outlet]
		UIKit.UIButton addEntryButton { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addEntryButton != null) {
				addEntryButton.Dispose ();
				addEntryButton = null;
			}

			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}
		}
	}
}
