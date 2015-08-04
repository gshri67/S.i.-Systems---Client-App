using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class TimeSheetEntryViewController : UIViewController
	{
		public TimeSheetEntryViewController (IntPtr handle) : base (handle)
		{
			NavigationItem.SetHidesBackButton (true, false);
		}

		public override void ViewWillLayoutSubviews ()
		{
			NavigationItem.SetHidesBackButton (true, false);
			//NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(), false);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			NavigationItem.SetHidesBackButton (false, false);

			base.PrepareForSegue (segue, sender);
		}
	}
}
