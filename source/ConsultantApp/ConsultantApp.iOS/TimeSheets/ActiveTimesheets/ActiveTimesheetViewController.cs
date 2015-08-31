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
using CoreGraphics;

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	partial class ActiveTimesheetViewController : UIViewController
	{
        private readonly NSObject _tokenExpiredObserver;
        private readonly ActiveTimesheetViewModel _activeTimesheetModel;
        private LoadingOverlay _overlay;
        public const string TimesheetSelectedSegue = "TimesheetSelected";

	    private IEnumerable<PayPeriod> _payPeriods;

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
	        IndicateLoading();
            if (_payPeriods == null)
                _payPeriods = await _activeTimesheetModel.GetPayPeriods();

            UpdateTableSource();
	    }

	    private void UpdateTableSource()
	    {
            RemoveOverlay();
	        InvokeOnMainThread(delegate
	        {
                if (_payPeriods == null) return;

                ActiveTimesheetsTable.Source = new ActiveTimesheetTableViewSource(this, _payPeriods);
                ActiveTimesheetsTable.ReloadData();
	        });
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

            LoadTimesheets();

            LogoutManager.CreateNavBarRightButton(this);
		}

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

	        LoadTimesheets();
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
                }
            }
        }

        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(ActiveTimesheets.Frame.X, ActiveTimesheets.Frame.Y, ActiveTimesheets.Frame.Width, ActiveTimesheets.Frame.Height);
                _overlay = new LoadingOverlay(frame, null);
                View.Add(_overlay);
            });
        }
        
        private void RemoveOverlay()
        {
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
        }
        #endregion
    }
}
