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
	[Register ("DisciplineViewController")]
	partial class DisciplineViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView DisciplineTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (DisciplineTable != null) {
				DisciplineTable.Dispose ();
				DisciplineTable = null;
			}
		}
	}
}
