using System;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController.AddTimeEntryViewController
{
	partial class AddTimeEntryViewController : UIViewController
	{
		public AddTimeEntryViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				//DismissViewController(true, null);
				NavigationController.PopViewController(true);
			};
		}
	}
}
