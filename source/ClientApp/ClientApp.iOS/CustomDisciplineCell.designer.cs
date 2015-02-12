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
		UIImageView ratingImage { get; set; }

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
			if (ratingImage != null) {
				ratingImage.Dispose ();
				ratingImage = null;
			}
		}
	}
}
