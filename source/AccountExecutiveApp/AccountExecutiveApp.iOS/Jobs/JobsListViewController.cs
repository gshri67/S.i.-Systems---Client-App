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
    public partial class JobsListViewController : UITableViewController
	{
		private readonly JobsListViewModel _jobsListViewModel;
        private SubtitleHeaderView _subtitleHeaderView;
	    public const string SubtitleCellIdentifier = "SubtitleWithRightDetailCell";
        public string Subtitle;
        private LoadingOverlay _overlay;
        private int _clientId;

        public JobsListViewController (IntPtr handle) : base (handle)
		{
			_jobsListViewModel = DependencyResolver.Current.Resolve<JobsListViewModel>();
		    Title = "Jobs";
		}

		private void SetupTableViewSource()
		{
			if (TableView == null)
				return;

            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleCellIdentifier);
            TableView.Source = new JobsListTableViewSource(this, _jobsListViewModel);

            TableView.ReloadData();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SearchManager.CreateNavBarRightButton(this);

            CreateCustomTitleBar();

            TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);

            RefreshControl = new UIRefreshControl();
            RefreshControl.Bounds = new CGRect(RefreshControl.Bounds.X, RefreshControl.Bounds.Y - 35, RefreshControl.Bounds.Width, RefreshControl.Bounds.Height);
            RefreshControl.ValueChanged += delegate
            {
                if (_overlay != null)
                    _overlay.Hidden = true;

                SetClientID(_clientId);
            };
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

		public void SetClientID( int ClientID )
		{
            if (RefreshControl == null || !RefreshControl.Refreshing)
                IndicateLoading();

		    _clientId = ClientID;

		    var task = _jobsListViewModel.SetClientID(ClientID);

            task.ContinueWith(_ => UpdateUserInterface());
		}

		public void UpdateUserInterface()
		{
            InvokeOnMainThread(SetupTableViewSource);
            InvokeOnMainThread(RemoveOverlay);
            InvokeOnMainThread(StopRefreshing);
        }

        public void StopRefreshing()
        {
            if (RefreshControl != null && RefreshControl.Refreshing)
                RefreshControl.EndRefreshing();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "JobSelectedSegue")
            {
                var destinationController = segue.DestinationViewController as JobDetailViewController;

                var source = TableView.Source as JobsListTableViewSource;

                var job = source.JobSelected(TableView.IndexPathForSelectedRow);

                destinationController.SetJobId(job.Id);

            }

            base.PrepareForSegue(segue, sender);
        }

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
                _subtitleHeaderView.TitleText = Title;
                _subtitleHeaderView.SubtitleText = Subtitle;
                NavigationItem.Title = "";
            });
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
