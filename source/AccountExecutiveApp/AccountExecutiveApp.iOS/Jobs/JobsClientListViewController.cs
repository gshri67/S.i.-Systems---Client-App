using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

namespace AccountExecutiveApp.iOS
{
    public partial class JobsClientListViewController : UITableViewController
	{
		private readonly JobsViewModel _jobsViewModel;
        private LoadingOverlay _overlay;
        private const string ClientSelectedFromJobListSegueIdentifier = "ClientSelectedSegue";

        public const string CellReuseIdentifier = "JobsClientListCell";

		public JobsClientListViewController (IntPtr handle) : base (handle)
		{
            _jobsViewModel = DependencyResolver.Current.Resolve<JobsViewModel>();

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), CellReuseIdentifier);

            if (NavigationController != null)
            {
                this.NavigationController.TabBarItem.SelectedImage = UIImage.FromFile("tag-dark.png");
            }
		}

		private void InstantiateTableViewSource()
		{
            if (TableView == null) return;
            
            TableView.Source = new JobsClientListTableViewSource(this, _jobsViewModel.Jobs);
            TableView.ReloadData();
			//TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            if( NavigationController != null )
    			NavigationController.SetNavigationBarHidden (false, false);

            //only show logout button if navigated from tab bar, not dashboard
		    if (TabBarController != null && TabBarController.SelectedIndex == 1)
		        LogoutManager.CreateNavBarLeftButton(this);
		    
            SearchManager.CreateNavBarRightButton(this);

		    LoadJobs();

            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += delegate
            {
                if (_overlay != null)
                    _overlay.Hidden = true;

                LoadJobs();
            };
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			NavigationController.SetNavigationBarHidden (false, false);
		}

        public void UpdateUserInterface()
        {
            InvokeOnMainThread(InstantiateTableViewSource);
            InvokeOnMainThread(RemoveOverlay);
            InvokeOnMainThread(StopRefreshing);
        }

        public void StopRefreshing()
        {
            if (RefreshControl != null && RefreshControl.Refreshing)
                RefreshControl.EndRefreshing();
        }

        public void LoadJobs()
        {
            if( RefreshControl == null || !RefreshControl.Refreshing )
            IndicateLoading();
            var task = _jobsViewModel.LoadJobs();
            task.ContinueWith(_ => UpdateUserInterface());
        }

	    public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
	    {
            if (segueIdentifier == ClientSelectedFromJobListSegueIdentifier)
            {
                return false;
            }

            return base.ShouldPerformSegue(segueIdentifier, sender);
	    }


        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);
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
