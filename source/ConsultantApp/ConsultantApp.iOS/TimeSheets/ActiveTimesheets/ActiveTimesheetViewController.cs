using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultantApp.Core.ViewModels;
using ConsultantApp.iOS.TimeEntryViewController;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;
using ConsultantApp.iOS.TimeEntryViewController;
using CoreGraphics;
using System.Linq;
using Shared.Core;


namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	partial class ActiveTimesheetViewController : UIViewController
	{
        private readonly NSObject _tokenExpiredObserver;
        private readonly ActiveTimesheetViewModel _activeTimesheetModel;
        private readonly ConsultantDetailsViewModel _consultantDetailsViewModel;
        private LoadingOverlay _overlay;
        public const string TimesheetSelectedSegue = "TimesheetSelected";
	    private const string ScreenTitle = "Timesheets";
		private SubtitleHeaderView subtitleHeaderView;

	    public ActiveTimesheetViewController (IntPtr handle) : base (handle)
		{
            _activeTimesheetModel = DependencyResolver.Current.Resolve<ActiveTimesheetViewModel>();
            _consultantDetailsViewModel = DependencyResolver.Current.Resolve<ConsultantDetailsViewModel>();
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);

			TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

            InitiatePayPeriodLoading();
	        InitiateConsultantDetailsLoading();
		}


        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
        }

	    public void LoadTimesheets()
	    {
            RemoveOverlay();

	        UpdateTableSource();

	        noTimesheetsLabel.Hidden = _activeTimesheetModel.UserHasPayPeriods();
	    }

	    private void UpdateTableSource()
	    {
	        if (!_activeTimesheetModel.UserHasPayPeriods())
	                return;
            
            ActiveTimesheetsTable.Source = new ActiveTimesheetTableViewSource(this, _activeTimesheetModel.PayPeriods);
            ActiveTimesheetsTable.ReloadData();
	    }

		private void CreateCustomTitleBar()
		{
			InvokeOnMainThread(() =>
				{
					subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = subtitleHeaderView;
					subtitleHeaderView.TitleText = ScreenTitle;
					subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName ?? string.Empty;
					NavigationItem.Title = "";
				});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

            IndicateLoading();

            CreateCustomTitleBar();

		    LogoutManager.CreateNavBarRightButton(this);
		}

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

	        InitiatePayPeriodLoading();
	    }

	    private void InitiatePayPeriodLoading()
	    {
            _activeTimesheetModel.LoadPayPayeriods();
            _activeTimesheetModel.LoadingPayPeriods.ContinueWith(_ => InvokeOnMainThread(LoadTimesheets));
	    }

	    private void InitiateConsultantDetailsLoading()
	    {
	        _consultantDetailsViewModel.LoadConsultantDetails();
            _consultantDetailsViewModel.LoadingConsultantDetails.ContinueWith(_ => InvokeOnMainThread(CreateCustomTitleBar));
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
