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
	[Register ("ConfirmContractViewController")]
	partial class ConfirmContractViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ApproverEmailLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ContractorRateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel EndDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ServicesRateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel SpecializationLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel StartDateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TotalRateLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ApproverEmailLabel != null) {
				ApproverEmailLabel.Dispose ();
				ApproverEmailLabel = null;
			}
			if (ContractorRateLabel != null) {
				ContractorRateLabel.Dispose ();
				ContractorRateLabel = null;
			}
			if (EndDateLabel != null) {
				EndDateLabel.Dispose ();
				EndDateLabel = null;
			}
			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
			if (ServicesRateLabel != null) {
				ServicesRateLabel.Dispose ();
				ServicesRateLabel = null;
			}
			if (SpecializationLabel != null) {
				SpecializationLabel.Dispose ();
				SpecializationLabel = null;
			}
			if (StartDateLabel != null) {
				StartDateLabel.Dispose ();
				StartDateLabel = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
			if (TotalRateLabel != null) {
				TotalRateLabel.Dispose ();
				TotalRateLabel = null;
			}
		}
	}
}
