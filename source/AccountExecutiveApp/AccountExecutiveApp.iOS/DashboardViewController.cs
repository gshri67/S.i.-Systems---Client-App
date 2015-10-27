using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
	partial class DashboardViewController : UIViewController
	{
		private readonly NSObject _tokenExpiredObserver;
		private DashboardViewModel _dashboardViewmodel;
		private DashboardInfo _dashboardInfo;

		public DashboardViewController (IntPtr handle) : base (handle)
		{
			this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);

			_dashboardViewmodel = DependencyResolver.Current.Resolve<DashboardViewModel>();

			//TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			//TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

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
			base.ViewDidLoad();


			LoadDashboardInfo ();

			//IndicateLoading();

			//CreateCustomTitleBar();

			LogoutManager.CreateNavBarLeftButton(this);

			View.LayoutIfNeeded ();
			View.NeedsUpdateConstraints ();

			FS_curContractsLabel.Layer.BorderWidth = 3;
			FS_curContractsLabel.Layer.BorderColor = UIColor.DarkGray.CGColor;
			FS_curContractsLabel.Layer.CornerRadius = FS_curContractsLabel.Frame.Width/2;

			FT_curContractsLabel.Layer.BorderWidth = 3;
			FT_curContractsLabel.Layer.BorderColor = UIColor.DarkGray.CGColor;
			FT_curContractsLabel.Layer.CornerRadius = FS_curContractsLabel.Frame.Width/2;

			FT_containerView.Layer.BorderWidth = 1;
			FT_containerView.Layer.BorderColor = UIColor.LightGray.CGColor;

			FS_containerView.Layer.BorderWidth = 1;
			FS_containerView.Layer.BorderColor = UIColor.LightGray.CGColor;

			FS_endingContractsLabel.Text = "88";
			FS_startingContractsLabel.Text = "88";

			UpdateUI ();
		}

		public void UpdateUI()
		{
			if (_dashboardInfo != null) 
			{
				Console.WriteLine (_dashboardInfo.FS_curContracts);

				FS_endingContractsLabel.Text = _dashboardInfo.FS_endingContracts.ToString();
				FS_startingContractsLabel.Text = _dashboardInfo.FS_startingContracts.ToString();
				FS_curContractsLabel.Text = _dashboardInfo.FS_curContracts.ToString();

				FT_endingContractsLabel.Text = _dashboardInfo.FT_endingContracts.ToString();
				FT_startingContractsLabel.Text = _dashboardInfo.FT_startingContracts.ToString();
				FT_curContractsLabel.Text = _dashboardInfo.FT_curContracts.ToString();

				jobsLabel.Text = _dashboardInfo.curJobs.ToString ();
				proposedJobsLabel.Text = _dashboardInfo.proposedJobs.ToString ();
				calloutJobsLabel.Text = _dashboardInfo.calloutJobs.ToString ();
			}
		}

		public async void LoadDashboardInfo()
		{
			if (_dashboardInfo != null) return;

			_dashboardInfo = await _dashboardViewmodel.getDashboardInfo ();

			UpdateUI();
		}
	}
}
