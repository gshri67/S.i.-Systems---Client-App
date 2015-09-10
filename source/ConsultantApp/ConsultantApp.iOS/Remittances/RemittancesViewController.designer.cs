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

namespace ConsultantApp.iOS
{
	[Register ("RemittancesViewController")]
	partial class RemittancesViewController
	{
		[Outlet]
		UIKit.UILabel noRemittancesLabel { get; set; }

		[Outlet]
		UIKit.UIView tableColumnHeaderView { get; set; }

		[Outlet]
		UIKit.UITableView tableview { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView RemitanceView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (RemitanceView != null) {
				RemitanceView.Dispose ();
				RemitanceView = null;
			}
		}
	}
}
