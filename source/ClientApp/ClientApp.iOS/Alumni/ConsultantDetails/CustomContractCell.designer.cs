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
	[Register ("CustomContractCell")]
	partial class CustomContractCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ActiveLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DirectReportLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel RateLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ActiveLabel != null) {
				ActiveLabel.Dispose ();
				ActiveLabel = null;
			}
			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}
			if (DirectReportLabel != null) {
				DirectReportLabel.Dispose ();
				DirectReportLabel = null;
			}
			if (RateLabel != null) {
				RateLabel.Dispose ();
				RateLabel = null;
			}
		}
	}
}
