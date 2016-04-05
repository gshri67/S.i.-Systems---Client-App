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
        private readonly CurrentConsultantDetailsViewModel _currentConsultantDetailsViewModel;
        private LoadingOverlay _overlay;
        public const string TimesheetSelectedSegue = "TimesheetSelected";
	    private const string ScreenTitle = "Timesheets";
		private SubtitleHeaderView _subtitleHeaderView;

	    public ActiveTimesheetViewController (IntPtr handle) : base (handle)
		{
            _activeTimesheetModel = DependencyResolver.Current.Resolve<ActiveTimesheetViewModel>();
            _currentConsultantDetailsViewModel = DependencyResolver.Current.Resolve<CurrentConsultantDetailsViewModel>();
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);

			TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");
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
#if TEST
            Console.WriteLine("LoadTimesheets");
#endif
            RemoveOverlay();
            
	        UpdateTableSource();

	        noTimesheetsLabel.Hidden = _activeTimesheetModel.UserHasPayPeriods();
#if TEST
            Console.WriteLine("Load Timesheets completed");
#endif
	    }

	    private void UpdateTableSource()
        {
#if TEST
            Console.WriteLine("UpdateTableSource");
#endif
            InvokeOnMainThread(delegate
            {
                ActiveTimesheetsTable.Source = new ActiveTimesheetTableViewSource(this, _activeTimesheetModel.PayPeriods);
                ActiveTimesheetsTable.ReloadData();
            });
#if TEST
            Console.WriteLine("Table Source Updated");
#endif
	    }

		private void CreateCustomTitleBar()
        {
#if TEST
            Console.WriteLine("CreateTitleBar");
#endif
            InvokeOnMainThread(() =>
				{
					_subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = _subtitleHeaderView;
					_subtitleHeaderView.TitleText = ScreenTitle;
					_subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName;
					NavigationItem.Title = "";
				});
#if TEST
            Console.WriteLine("Title bar created.");
#endif
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
         
            CreateCustomTitleBar();

		    LogoutManager.CreateNavBarRightButton(this);
		}

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

            IndicateLoading();

            InitiatePayPeriodLoading();
            InitiateConsultantDetailsLoading();
	    }

	    private void InitiatePayPeriodLoading()
	    {
            var loadPeriodsTask = _activeTimesheetModel.LoadPayPeriods();
            loadPeriodsTask.ContinueWith(_ => LoadTimesheets());
	    }

	    private void InitiateConsultantDetailsLoading()
	    {
	        _currentConsultantDetailsViewModel.LoadConsultantDetails();
            _currentConsultantDetailsViewModel.LoadingConsultantDetails.ContinueWith(_ => CreateCustomTitleBar());
	    }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == TimesheetSelectedSegue)
            {
                var navCtrl = segue.DestinationViewController as TimesheetOverviewViewController;

                if (navCtrl == null) return;

                //pass the selected item into the Overview Controller
                var source = ActiveTimesheetsTable.Source as ActiveTimesheetTableViewSource;
                var rowpath = ActiveTimesheetsTable.IndexPathForSelectedRow;
                var timesheet = source.GetItem(rowpath);
                navCtrl.LoadTimesheet(timesheet);
            }
        }

        #region Overlay

        private void IndicateLoading()
        {
#if TEST
            Console.WriteLine("Indicate loading");
#endif
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;

                _overlay = new LoadingOverlay(View.Bounds, null);
                _overlay.BackgroundColor = UIColor.FromWhiteAlpha(1.0f, 0.5f);
                _overlay.UserInteractionEnabled = false;
                View.Add(_overlay);
            });
#if TEST
            Console.WriteLine("Finished Indicate loading");
#endif
        }
        
        private void RemoveOverlay()
        {
#if TEST
            Console.WriteLine("Remove Overlay");
#endif
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
#if TEST
            Console.WriteLine("Overlay Removed");
#endif
        }
        #endregion
    }
}
