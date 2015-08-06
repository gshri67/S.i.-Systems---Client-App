using System;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController.AddTimeEntryViewController
{
	partial class AddTimeEntryViewController : UIViewController
	{
		public AddTimeEntryViewController (IntPtr handle) : base (handle)
		{
			addButton.TouchUpInside += delegate {
				DismissViewController(true, null);
			};
		}
	}
}
