﻿// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AccountExecutiveApp.iOS
{
	[Register ("SearchTableViewController")]
	partial class SearchTableViewController
	{
		[Outlet]
		UIKit.UIButton CancelSearchButton { get; set; }

		[Outlet]
		UIKit.UISearchBar SearchBar { get; set; }

		[Outlet]
		UIKit.UIView SearchHeaderView { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelSearchButton != null) {
				CancelSearchButton.Dispose ();
				CancelSearchButton = null;
			}

			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (SearchHeaderView != null) {
				SearchHeaderView.Dispose ();
				SearchHeaderView = null;
			}
		}
	}
}
