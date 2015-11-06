using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
	partial class DashboardViewController : UIViewController
	{
		private readonly NSObject _tokenExpiredObserver;
		private readonly DashboardViewModel _dashboardViewmodel;
        private LoadingOverlay _overlay;

		public DashboardViewController (IntPtr handle) : base (handle)
		{
			this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);

			_dashboardViewmodel = DependencyResolver.Current.Resolve<DashboardViewModel>();
		}

		public void OnTokenExpired(NSNotification notifcation)
		{
			// Unsubscribe from this event so that it won't be handled again
			NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

			var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
			var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

			UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
		}

	    private void AddCircularBorderToLabel(UILabel label)
	    {
            label.Layer.BorderWidth = 3;
            label.Layer.BorderColor = UIColor.DarkGray.CGColor;
            label.Layer.CornerRadius = label.Frame.Width / 2;
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
            
			LoadDashboardInfo();

			IndicateLoading();

			LogoutManager.CreateNavBarLeftButton(this);

			SetupPageAutoLayout();
		}

	    private void SetupPageAutoLayout()
	    {
	        View.LayoutIfNeeded();
	        View.NeedsUpdateConstraints();

	        AddCircularBorderToLabel(FS_curContractsLabel);
	        AddCircularBorderToLabel(FT_curContractsLabel);
            AddBorderToView(FT_containerView);
            AddBorderToView(FS_containerView);
	    }

	    private void AddBorderToView(UIView view)
	    {
            view.Layer.BorderWidth = 1;
            view.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;
	    }

	    private void SetFullySourcedLabels()
	    {
            FS_endingContractsLabel.Text = _dashboardViewmodel.FullySourcedEndingContracts();
            FS_startingContractsLabel.Text = _dashboardViewmodel.FullySourcedStartingContracts();
            FS_curContractsLabel.Text = _dashboardViewmodel.FullySourcedCurrentContracts();
	    }

	    private void SetFlowThruLabels()
	    {
            FT_endingContractsLabel.Text = _dashboardViewmodel.FloThruEndingContracts();
            FT_startingContractsLabel.Text = _dashboardViewmodel.FloThruStartingContracts();
            FT_curContractsLabel.Text = _dashboardViewmodel.FlowThruCurrentContracts();    
	    }

	    private void SetJobsLabels()
	    {
            jobsLabel.Text = _dashboardViewmodel.AllJobs();
            proposedJobsLabel.Text = _dashboardViewmodel.ProposedJobs();
            calloutJobsLabel.Text = _dashboardViewmodel.JobsWithCallouts();
	    }

		private void UpdateUserInterface()
		{
		    InvokeOnMainThread(delegate
		    {
		        SetFullySourcedLabels();
		        SetFlowThruLabels();
		        SetJobsLabels();
		        RemoveOverlay();
		    });
		}

	    private void NotifyOfError()
	    {
	        //todo: Add any error alerts and display that screen as appropriate
	    }

		private void LoadDashboardInfo()
		{
            _dashboardViewmodel.LoadDashboardInformation();
		    _dashboardViewmodel.DashboardIsLoading.ContinueWith(_ => UpdateUserInterface(), TaskContinuationOptions.OnlyOnRanToCompletion);
            _dashboardViewmodel.DashboardIsLoading.ContinueWith(_ => NotifyOfError(), TaskContinuationOptions.NotOnRanToCompletion);
		}

        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(DashboardView.Frame.X, DashboardView.Frame.Y, DashboardView.Frame.Width, DashboardView.Frame.Height);
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
