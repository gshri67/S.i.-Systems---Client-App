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
		UILabel ContractShortNameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel RateLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContractShortNameLabel != null) {
				ContractShortNameLabel.Dispose ();
				ContractShortNameLabel = null;
			}
			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}
			if (RateLabel != null) {
				RateLabel.Dispose ();
				RateLabel = null;
			}
		}
	}
}
