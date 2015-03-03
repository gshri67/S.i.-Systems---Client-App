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
		UITableViewCell ContractCell { get; set; }

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
		UITableViewCell SpecializationCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel SpecializationLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell StartDateCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel StartDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell TimeSheetCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField TitleField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContractCell != null) {
				ContractCell.Dispose ();
				ContractCell = null;
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
			if (SpecializationCell != null) {
				SpecializationCell.Dispose ();
				SpecializationCell = null;
			}
			if (SpecializationLabel != null) {
				SpecializationLabel.Dispose ();
				SpecializationLabel = null;
			}
			if (StartDateCell != null) {
				StartDateCell.Dispose ();
				StartDateCell = null;
			}
			if (StartDateLabel != null) {
				StartDateLabel.Dispose ();
				StartDateLabel = null;
			}
			if (TimeSheetCell != null) {
				TimeSheetCell.Dispose ();
				TimeSheetCell = null;
			}
			if (TitleField != null) {
				TitleField.Dispose ();
				TitleField = null;
			}
		}
	}
}
