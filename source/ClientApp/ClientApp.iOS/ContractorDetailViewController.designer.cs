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
	[Register ("ContractorDetailViewController")]
	partial class ContractorDetailViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ContactButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ContractsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel RatingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell SpecializationCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

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
