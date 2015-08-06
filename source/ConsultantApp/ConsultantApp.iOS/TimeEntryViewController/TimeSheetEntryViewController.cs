using System;
using Foundation;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	partial class TimeSheetEntryViewController : UIViewController
	{
		public TimeSheetEntryViewController (IntPtr handle) : base (handle)
		{
			addButton.TouchUpInside += delegate {
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");

				vc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				PresentModalViewController(vc, true);
			};
		}

		public override void ViewWillLayoutSubviews ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.PushViewController( NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false );
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
