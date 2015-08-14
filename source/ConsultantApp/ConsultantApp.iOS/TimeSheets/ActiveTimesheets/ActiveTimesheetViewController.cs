using System;
using System.Collections;
using System.Collections.Generic;
using ConsultantApp.Core.ViewModels;
using ConsultantApp.iOS.TimeEntryViewController;
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
        public const string TimesheetSelectedSegue = "TimesheetSelected";

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
            var payPeriods = await _activeTimesheetModel.GetPayPeriods();

            UpdateTableSource(payPeriods);
	    }

	    private void UpdateTableSource(IEnumerable<PayPeriod> payPeriods)
	    {
	        InvokeOnMainThread(delegate
	        {
	            if (payPeriods == null) return;

                ActiveTimesheetsTable.Source = new ActiveTimesheetTableViewSource(this, payPeriods);
                ActiveTimesheetsTable.ReloadData();
	        });
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LoadTimesheets();

            LogoutManager.CreateNavBarRightButton(this);
		}

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == TimesheetSelectedSegue)
            {
                var navCtrl = segue.DestinationViewController as TimesheetOverviewViewController;

                if (navCtrl != null)
                {
                    //pass the selected item into the Overview Controller
                    var source = ActiveTimesheetsTable.Source as ActiveTimesheetTableViewSource;
                    var rowpath = ActiveTimesheetsTable.IndexPathForSelectedRow;
                    var timesheet = source.GetItem(rowpath);
                    navCtrl.SetTimesheet(timesheet);
                    //navCtrl.SetSpecialization(this, consultantGroup, _tableSelector.SelectedSegment != AlumniSelected);
                }
            }
        }
	}
}
