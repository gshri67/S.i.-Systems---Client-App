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
	[Register ("CustomAlumniCell")]
	partial class CustomAlumniCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel alumniCountLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel specializationLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (alumniCountLabel != null) {
				alumniCountLabel.Dispose ();
				alumniCountLabel = null;
			}
			if (specializationLabel != null) {
				specializationLabel.Dispose ();
				specializationLabel = null;
			}
		}
	}
}
