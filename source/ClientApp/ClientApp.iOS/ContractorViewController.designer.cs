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
	[Register ("ContractorViewController")]
	partial class ContractorViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem AdditionalActions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchBar contractorSearch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SpecializationTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel summaryLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AdditionalActions != null) {
				AdditionalActions.Dispose ();
				AdditionalActions = null;
			}
			if (contractorSearch != null) {
				contractorSearch.Dispose ();
				contractorSearch = null;
			}
			if (SpecializationTable != null) {
				SpecializationTable.Dispose ();
				SpecializationTable = null;
			}
			if (summaryLabel != null) {
				summaryLabel.Dispose ();
				summaryLabel = null;
			}
		}
	}
}
