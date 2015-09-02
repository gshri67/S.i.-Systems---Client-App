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
	[Register ("RemittancesViewController")]
	partial class RemittancesViewController
	{
		[Outlet]
		UIKit.UIView tableColumnHeaderView { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableview != null) {
				tableview.Dispose ();
				tableview = null;
			}

			if (tableColumnHeaderView != null) {
				tableColumnHeaderView.Dispose ();
				tableColumnHeaderView = null;
			}
		}
	}
}
