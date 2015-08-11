using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Shared.Core;
using SiSystems.SharedModels;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	public partial class TimeSheetEntryViewController : UIViewController
	{
        private readonly NSObject _tokenExpiredObserver;
		private LogoutViewModel logoutViewModel;

		public TimeSheetEntryViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

            TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
            TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

			CreateNavBarLeftButton ();

			logoutViewModel = DependencyResolver.Current.Resolve<LogoutViewModel> ();

            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
		}


		private void CreateNavBarLeftButton()
		{
			var buttonImage = UIImage.FromBundle("app-button").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
			NavigationItem.SetLeftBarButtonItem(
				new UIBarButtonItem(buttonImage
					, UIBarButtonItemStyle.Plain
					, (sender, args) =>
					{
						AdditionalActions_Pressed();
					})
				, true);
		}

		private void AdditionalActions_Pressed()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
				var logoutAction = UIAlertAction.Create("Logout", UIAlertActionStyle.Destructive, LogoutDelegate);
				var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
				controller.AddAction(logoutAction);
				controller.AddAction(cancelAction);
				PresentViewController(controller, true, null);
			}
			else
			{
				var sheet = new UIActionSheet();
				sheet.AddButton("Logout");
				sheet.AddButton("Cancel");
				sheet.DestructiveButtonIndex = 0;
				sheet.CancelButtonIndex = 1;
				sheet.Clicked += LogoutDelegate;
				sheet.ShowFromTabBar(NavigationController.TabBarController.TabBar);// ShowInView(View);
			}
		}

		private void LogoutDelegate(UIAlertAction action)
		{
			logoutViewModel.Logout();
			//_alumniModel.Logout();
			/*
			var rootController =
				UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle)
					.InstantiateViewController("LoginView");
			var navigationController = new UINavigationController(rootController)
			{
				NavigationBarHidden = true
			};
			UIApplication.SharedApplication.Windows[0].RootViewController =
				navigationController;*/

			NavigationController.PushViewController(NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false);
		}

		private void LogoutDelegate(object sender, UIButtonEventArgs args)
		{
			if (args.ButtonIndex == 0)
			{
				LogoutDelegate(null);
			}
		}


        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
        }
			
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Test data, normally we would load all the timeentries for this day from API
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
            NavigationController.PushViewController(NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false);
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			/*
			TimeEntryCell currentCell = (TimeEntryCell)(tableview.CellAt(tableview.IndexPathForSelectedRow));
			if( currentCell != null )
				currentCell.hoursField.ResignFirstResponder();*/
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
