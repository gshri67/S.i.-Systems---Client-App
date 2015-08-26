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

namespace ConsultantApp.iOS
{
	[Register ("ResetPasswordViewController")]
	partial class ResetPasswordViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem CancelBarButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField emailAddressField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SubmitButton { get; set; }

		[Action ("CancelBarButton_Activated:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CancelBarButton_Activated (UIBarButtonItem sender);

		[Action ("SubmitButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SubmitButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}
			if (CancelBarButton != null) {
				CancelBarButton.Dispose ();
				CancelBarButton = null;
			}
			if (emailAddressField != null) {
				emailAddressField.Dispose ();
				emailAddressField = null;
			}
			if (SubmitButton != null) {
				SubmitButton.Dispose ();
				SubmitButton = null;
			}
		}
	}
}
