using System;
using Foundation;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	partial class TimeSheetEntryViewController : UIViewController
	{
		public TimeSheetEntryViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

			TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			//TabBarController.TabBar.Items [0].Image = (new UIImage ("test3.png"));
			TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");
			//TabBarItem.Image = (new UIImage ("ios7-clock-outline.png")).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
			/*
			TabBarController.TabBar.TintColor = UIColor.White;
			TabBarController.TabBar.BarTintColor = UIColor.White;
			TabBarController.TabBar.BackgroundColor = UIColor.White;*/
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
