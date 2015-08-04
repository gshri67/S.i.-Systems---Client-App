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
		BorderedButton ContactButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ContractsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView DetailsTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView LeftStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView MiddleStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		BorderedButton OnboardButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel RatingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableViewCell ResumeCell { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ResumeLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView RightStar { get; set; }

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
			if (DetailsTable != null) {
				DetailsTable.Dispose ();
				DetailsTable = null;
			}
			if (LeftStar != null) {
				LeftStar.Dispose ();
				LeftStar = null;
			}
			if (MiddleStar != null) {
				MiddleStar.Dispose ();
				MiddleStar = null;
			}
			if (OnboardButton != null) {
				OnboardButton.Dispose ();
				OnboardButton = null;
			}
			if (RatingLabel != null) {
				RatingLabel.Dispose ();
				RatingLabel = null;
			}
			if (ResumeCell != null) {
				ResumeCell.Dispose ();
				ResumeCell = null;
			}
			if (ResumeLabel != null) {
				ResumeLabel.Dispose ();
				ResumeLabel = null;
			}
			if (RightStar != null) {
				RightStar.Dispose ();
				RightStar = null;
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
