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
	[Register ("CustomDisciplineCell")]
	partial class CustomDisciplineCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel contractDates { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel fullName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lastContractRate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView LeftStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView MiddleStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView RightStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel UncheckedLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (contractDates != null) {
				contractDates.Dispose ();
				contractDates = null;
			}
			if (fullName != null) {
				fullName.Dispose ();
				fullName = null;
			}
			if (lastContractRate != null) {
				lastContractRate.Dispose ();
				lastContractRate = null;
			}
			if (LeftStar != null) {
				LeftStar.Dispose ();
				LeftStar = null;
			}
			if (MiddleStar != null) {
				MiddleStar.Dispose ();
				MiddleStar = null;
			}
			if (RightStar != null) {
				RightStar.Dispose ();
				RightStar = null;
			}
			if (UncheckedLabel != null) {
				UncheckedLabel.Dispose ();
				UncheckedLabel = null;
			}
		}
	}
}
