using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.Core;
using CoreGraphics;
using HealthKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class TimesheetListTableViewController : UITableViewController
	{
		private readonly TimesheetListViewModel _viewModel;
	    private LoadingOverlay _overlay;

	    public TimesheetListTableViewController (IntPtr handle) : base (handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<TimesheetListViewModel>();
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null) return;

			RegisterCellsForReuse();
			TableView.Source = new TimesheetListTableViewSource(this, _viewModel.Timesheets, _viewModel.Status);
			TableView.ReloadData();
			//TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SearchManager.CreateNavBarRightButton(this);

            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += delegate
            {
                if (_overlay != null)
                    _overlay.Hidden = true;

                LoadTimesheetDetails(_viewModel.Status);
            };
		}

		public override void ViewDidAppear (bool animated)
		{
		}

		public void UpdateUserInterface()
		{
            InvokeOnMainThread(RemoveOverlay);
			InvokeOnMainThread(InstantiateTableViewSource);
			InvokeOnMainThread(UpdateTitle);
            InvokeOnMainThread(StopRefreshing);
        }

        public void StopRefreshing()
        {
            if (RefreshControl != null && RefreshControl.Refreshing)
                RefreshControl.EndRefreshing();
		}

	    public void UpdateTitle()
	    {
            Title = _viewModel.PageTitle;
	    }

	    public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public void LoadTimesheetDetails( MatchGuideConstants.TimesheetStatus status )
		{
            if( RefreshControl == null || !RefreshControl.Refreshing )
                IndicateLoading();
			
            var task = _viewModel.LoadTimesheetDetails( status );
			task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
		}


        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(TableView.Frame.X, TableView.Frame.Y, TableView.Frame.Width, TableView.Frame.Height);
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

