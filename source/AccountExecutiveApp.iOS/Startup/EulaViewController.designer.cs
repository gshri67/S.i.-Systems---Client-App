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
	[Register ("EulaViewController")]
	partial class EulaViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton agree { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView eulaText { get; set; }

		[Action ("agree_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void agree_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (agree != null) {
				agree.Dispose ();
				agree = null;
			}
			if (eulaText != null) {
				eulaText.Dispose ();
				eulaText = null;
			}
		}
	}
}
