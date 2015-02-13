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
	[Register ("ContractsViewController")]
	partial class ContractsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView ContractsTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContractsTable != null) {
				ContractsTable.Dispose ();
				ContractsTable = null;
			}
		}
	}
}
