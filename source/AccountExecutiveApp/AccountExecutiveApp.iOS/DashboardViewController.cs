using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using AccountExecutiveApp.iOS;

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

			SetupSegueConnections ();

			SetupPageAutoLayout();


			AddImageToLabel( new UIImage ("ios7-plus-empty-centred.png"), FS_plusLabel);
			AddImageToLabel( new UIImage ("ios7-plus-empty-centred.png"), FT_plusLabel);
			AddImageToLabel( new UIImage ("ios7-minus-empty-centred.png"), FS_MinusLabel);
			AddImageToLabel( new UIImage ("ios7-minus-empty-centred.png"), FT_minusLabel);
		}

		private void AddImageToLabel( UIImage image, UILabel label )
		{
			NSTextAttachment textAttachement = new NSTextAttachment ();
			textAttachement.Image = image;
			textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, label.Bounds.Size.Width, label.Bounds.Size.Height);
			NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);
			label.AttributedText = attrStringWithImage;
		}

		private void SetupSegueConnections ()
		{
			FS_endingButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Ending;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.Consultant;
				ShowViewController( contractListVC, this );
			};
			FS_startingButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Starting;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.Consultant;
                ShowViewController(contractListVC, this);
			};
			FS_activeButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Active;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.Consultant;
				ShowViewController( contractListVC, this );
			};
			FT_endingButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Ending;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.FloThru;
				ShowViewController( contractListVC, this );
			};
			FT_startingButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Starting;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.FloThru;
				ShowViewController( contractListVC, this );
			};
			FT_activeButtonOverlay.TouchUpInside += delegate 
			{
				var contractListVC = (ContractsListViewController)Storyboard.InstantiateViewController("ContractsListViewController");
                contractListVC.StatusType = ContractStatusType.Active;
                contractListVC.TypeOfContract = MatchGuideConstants.AgreementSubTypes.FloThru;
				ShowViewController( contractListVC, this );
			};

			var jobsListVC = (JobsClientListViewController)Storyboard.InstantiateViewController("JobsClientListViewController");

			JobsButtonOverlay.TouchUpInside += delegate 
			{
				//contractListVC.setContracts( _dashboardViewmodel.EndingFullySourcedContracts );
				ShowViewController( jobsListVC, this );
			};
		}

	    private void SetupPageAutoLayout()
	    {
	        View.LayoutIfNeeded();
	        View.NeedsUpdateConstraints();

	        AddCircularBorderToLabel(FS_curContractsLabel);
	        AddCircularBorderToLabel(FT_curContractsLabel);
            AddBorderToView(FT_containerView);
            AddBorderToView(FS_containerView);
			AddBorderToView(jobsContainerView);
	    }

	    private void AddBorderToView(UIView view)
	    {
            view.Layer.BorderWidth = 1;
            view.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;
	    }

	    private void SetFullySourcedLabels()
	    {
	        FS_endingContractsLabel.Text = _dashboardViewmodel.EndingFullySourcedContracts;
	        FS_startingContractsLabel.Text = _dashboardViewmodel.StartingFullySourcedContracts;
	        FS_curContractsLabel.Text = _dashboardViewmodel.CurrentFullySourcedContracts;
	    }

	    private void SetFloThruLabels()
	    {
	        FT_endingContractsLabel.Text = _dashboardViewmodel.EndingFloThruContracts;
	        FT_startingContractsLabel.Text = _dashboardViewmodel.StartingFloThruContracts;
	        FT_curContractsLabel.Text = _dashboardViewmodel.CurrentFloThruContracts;
	    }

	    private void SetJobsLabels()
	    {
            jobsLabel.Text = _dashboardViewmodel.AllJobs;
            proposedJobsLabel.Text = _dashboardViewmodel.ProposedJobs;
            calloutJobsLabel.Text = _dashboardViewmodel.JobsWithCallouts;
	    }

		private void UpdateUserInterface()
		{
		    InvokeOnMainThread(delegate
		    {
		        SetFullySourcedLabels();
		        SetFloThruLabels();
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
            var loadDashboardTask = _dashboardViewmodel.LoadDashboardInformation();
		    loadDashboardTask.ContinueWith(_ => UpdateUserInterface(), TaskContinuationOptions.OnlyOnRanToCompletion);
            loadDashboardTask.ContinueWith(_ => NotifyOfError(), TaskContinuationOptions.NotOnRanToCompletion);
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
