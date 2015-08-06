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
		}

		public override void ViewWillLayoutSubviews ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");
				/*
				vc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

				PresentModalViewController(vc, true);
*/
				NavigationController.PushViewController(vc, true);
			};

			NavigationController.PushViewController( NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false );
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
