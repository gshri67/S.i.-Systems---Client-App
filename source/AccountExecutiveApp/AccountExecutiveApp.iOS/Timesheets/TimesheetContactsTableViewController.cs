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
	public partial class TimesheetContactsTableViewController : UITableViewController
	{
		private readonly TimesheetContactsViewModel _viewModel;
	    private SubtitleHeaderView _subtitleHeaderView;
        private bool _needsCreateTitleBar = false;
	    private LoadingOverlay _overlay;

	    public TimesheetContactsTableViewController(IntPtr handle)
            : base(handle)
		{
		    _viewModel = DependencyResolver.Current.Resolve<TimesheetContactsViewModel>();
		}
        
		private void InstantiateTableViewSource()
		{
			if (TableView == null) return;

			RegisterCellsForReuse();
			TableView.Source = new TimesheetContactsTableViewSource(this, _viewModel.Contact, _viewModel.Status );
			TableView.ReloadData();
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;

			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
            TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), ContractorContactInfoCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(ButtonCell), ButtonCell.CellIdentifier);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SearchManager.CreateNavBarRightButton(this);
		}

		public override void ViewDidAppear (bool animated)
		{
            CreateTitleBarIfNeeded();
		}

		public void UpdateUserInterface()
		{
            InvokeOnMainThread(RemoveOverlay);
            InvokeOnMainThread( CreateTitleBarIfNeeded );
			InvokeOnMainThread(InstantiateTableViewSource);
		}


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
        

		public void LoadTimesheetContactWithIdAndStatus( int Id, MatchGuideConstants.TimesheetStatus status )
		{
            IndicateLoading();
			var task = _viewModel.LoadTimesheetContactWithIdAndStatus( Id, status );

			task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
		}

        private void CreateTitleBarIfNeeded()
        {
            if (!_needsCreateTitleBar)
                _needsCreateTitleBar = true;
            else
                InvokeOnMainThread(CreateCustomTitleBar);
        }

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
                _subtitleHeaderView.TitleText = _viewModel.PageTitle;
                _subtitleHeaderView.SubtitleText = _viewModel.PageSubtitle;
                NavigationItem.Title = "";
            });
        }

	    public void RequestTimesheetApproval()
	    {
	        StartRequestTimesheetApprovalConfirmation();
	    }

        private void StartRequestTimesheetApprovalConfirmation()
        {
            var requestAlertController = UIAlertController.Create("Re-Request Approval", "Are you sure you want to request approval again for this timesheet? An email will be sent to the Direct Report.", UIAlertControllerStyle.Alert);

            //Add Actions
            requestAlertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alert =>
            {
                StartRequestingTimesheetApproval();
            }
            ));
            requestAlertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, alert => Console.WriteLine("No was clicked")));

            //Present Alert
            PresentViewController(requestAlertController, true, null);
        }

        private void StartRequestingTimesheetApproval()
        {
            IndicateLoading();

            var task = _viewModel.RequestTimesheetApproval();
            task.ContinueWith(_ => InvokeOnMainThread(EndTimesheetApprovalRequest), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        private void EndTimesheetApprovalRequest()
        {
            RemoveOverlay();
            DismissViewController(true, null);
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

