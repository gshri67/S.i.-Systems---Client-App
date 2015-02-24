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
	[Register ("NewContractViewController")]
	partial class NewContractViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField ApproverEmailField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell EndDateCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel EndDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView NewContractTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField RateField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ServiceLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell StartDateCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel StartDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TotalLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ApproverEmailField != null) {
				ApproverEmailField.Dispose ();
				ApproverEmailField = null;
			}
			if (EndDateCell != null) {
				EndDateCell.Dispose ();
				EndDateCell = null;
			}
			if (EndDateLabel != null) {
				EndDateLabel.Dispose ();
				EndDateLabel = null;
			}
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
			if (NewContractTable != null) {
				NewContractTable.Dispose ();
				NewContractTable = null;
			}
			if (RateField != null) {
				RateField.Dispose ();
				RateField = null;
			}
			if (ServiceLabel != null) {
				ServiceLabel.Dispose ();
				ServiceLabel = null;
			}
			if (StartDateCell != null) {
				StartDateCell.Dispose ();
				StartDateCell = null;
			}
			if (StartDateLabel != null) {
				StartDateLabel.Dispose ();
				StartDateLabel = null;
			}
			if (TotalLabel != null) {
				TotalLabel.Dispose ();
				TotalLabel = null;
			}
		}
	}
}
