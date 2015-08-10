using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Shared.Core;
using ConsultantApp.SharedModels;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	public partial class TimeSheetEntryViewController : UIViewController
	{
        private readonly NSObject _tokenExpiredObserver;

		public TimeSheetEntryViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

			TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
		}

        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
        }

		public override void ViewWillLayoutSubviews ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			List<TimeEntry> timeEntries = new List<TimeEntry>();
			TimeEntry t1 = new TimeEntry();
			t1.clientName = "Cenovus";
			t1.projectCode = "P-336";
			timeEntries.Add ( t1 );

			tableview.RegisterClassForCellReuse( typeof(TimeEntryCell), @"TimeEntryCell");
			tableview.Source = new TimeEntryTableViewSource (this, timeEntries);
			tableview.ReloadData ();

			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");
				/*
				vc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

				PresentModalViewController(vc, true);
*/
				NavigationController.PushViewController(vc, true);
			};
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
