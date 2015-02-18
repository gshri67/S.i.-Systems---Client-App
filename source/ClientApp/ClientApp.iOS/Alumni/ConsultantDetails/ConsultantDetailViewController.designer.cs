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
	[Register ("ConsultantDetailViewController")]
	partial class ConsultantDetailViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ContactButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ContractsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView DetailsTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton NewContractButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel RatingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell SpecializationCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		[Action ("NewContractButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void NewContractButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ContactButton != null) {
				ContactButton.Dispose ();
				ContactButton = null;
			}
			if (ContractsLabel != null) {
				ContractsLabel.Dispose ();
				ContractsLabel = null;
			}
			if (DetailsTable != null) {
				DetailsTable.Dispose ();
				DetailsTable = null;
			}
			if (NewContractButton != null) {
				NewContractButton.Dispose ();
				NewContractButton = null;
			}
			if (RatingLabel != null) {
				RatingLabel.Dispose ();
				RatingLabel = null;
			}
			if (SpecializationCell != null) {
				SpecializationCell.Dispose ();
				SpecializationCell = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
