using System;
using System.Collections;
using System.Collections.Generic;
using ConsultantApp.Core.ViewModels;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;
using ConsultantApp.iOS.TimeEntryViewController;

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	partial class ActiveTimesheetViewController : UIViewController
	{
        private readonly NSObject _tokenExpiredObserver;
	    private readonly ActiveTimesheetViewModel _activeTimesheetModel;

		public ActiveTimesheetViewController (IntPtr handle) : base (handle)
		{
            _activeTimesheetModel = DependencyResolver.Current.Resolve<ActiveTimesheetViewModel>();
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

	    public async void LoadTimesheets()
	    {
            var timesheets = await _activeTimesheetModel.GetCurrentActiveTimesheets();

            UpdateTableSource(timesheets);
	    }

	    private void UpdateTableSource(IEnumerable<Timesheet> timesheets)
	    {
	        InvokeOnMainThread(delegate
	        {
	            if (timesheets == null) return;

	            tableview.Source = new ActiveTimesheetTableViewSource(this, timesheets);
	            tableview.ReloadData();
	        });
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LoadTimesheets();

			NavigationController.NavigationBar.Translucent = false;

			//TimesheetOverviewViewController vc = (TimesheetOverviewViewController)Storyboard.InstantiateViewController ("TimesheetOverviewViewController");

			//if( vc != null )
			//	NavigationController.PushViewController ( vc, true );
		}
	}
}
