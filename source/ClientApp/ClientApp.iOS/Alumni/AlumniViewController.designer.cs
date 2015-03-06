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

namespace ClientApp.iOS
{
	[Register ("AlumniViewController")]
	partial class AlumniViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchBar AlumniSearch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView AlumniView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SpecializationTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AlumniSearch != null) {
				AlumniSearch.Dispose ();
				AlumniSearch = null;
			}
			if (AlumniView != null) {
				AlumniView.Dispose ();
				AlumniView = null;
			}
			if (SpecializationTable != null) {
				SpecializationTable.Dispose ();
				SpecializationTable = null;
			}
		}
	}
}
