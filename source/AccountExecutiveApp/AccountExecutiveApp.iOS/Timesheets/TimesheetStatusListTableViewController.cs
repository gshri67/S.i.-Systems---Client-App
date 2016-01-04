using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using HealthKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class TimesheetStatusListTableViewController : UITableViewController
	{
		private readonly TimesheetStatusListViewModel _viewModel;
	    private LoadingOverlay _overlay;

	    public TimesheetStatusListTableViewController () : base ("TimesheetStatusListTableViewController", null)
		{
		}

		public TimesheetStatusListTableViewController (IntPtr handle) : base (handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<TimesheetStatusListViewModel>();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LogoutManager.CreateNavBarLeftButton(this);
            SearchManager.CreateNavBarRightButton(this);

            TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);
		}

        public void LoadTimesheetSummary()
        {
            var task = _viewModel.LoadTimesheetSummary();

            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

	    private void UpdateUserInterface()
	    {
            RemoveOverlay();

            OpenTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfOpenTimesheets;
            SubmittedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfSubmittedTimesheets;
            RejectedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfRejectedTimesheets;
            CancelledTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfCancelledTimesheets;

            //Make cell not selectable if there are no timesheets available
	        if (_viewModel.FormattedNumberOfOpenTimesheets == "0")
	        {
	            OpenTimesheetsCell.UserInteractionEnabled = false;
                OpenTimesheetsCell.Accessory =  UITableViewCellAccessory.None;
	        }
	        if (_viewModel.FormattedNumberOfSubmittedTimesheets == "0")
	        {
	            SubmittedTimesheetsCell.UserInteractionEnabled = false;
                SubmittedTimesheetsCell.Accessory = UITableViewCellAccessory.None;
	        }
	        if (_viewModel.FormattedNumberOfRejectedTimesheets == "0")
            {
                RejectedTimesheetsCell.UserInteractionEnabled = false;
                RejectedTimesheetsCell.Accessory = UITableViewCellAccessory.None;
	        }
            if (_viewModel.FormattedNumberOfCancelledTimesheets == "0")
            {
                CancelledTimesheetsCell.UserInteractionEnabled = false;
                CancelledTimesheetsCell.Accessory = UITableViewCellAccessory.None;
	        }
	    }

	    public override void ViewDidAppear (bool animated)
		{
            IndicateLoading();
            LoadTimesheetSummary();
		}

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var destinationController = segue.DestinationViewController as TimesheetListTableViewController;

            if (segue.Identifier == "OpenTimesheetsSelectedSegue")
                destinationController.LoadTimesheetDetails(MatchGuideConstants.TimesheetStatus.Open);

            else if (segue.Identifier == "SubmittedTimesheetsSelectedSegue")
                destinationController.LoadTimesheetDetails(MatchGuideConstants.TimesheetStatus.Submitted);

            else if (segue.Identifier == "CancelledTimesheetsSelectedSegue")
                destinationController.LoadTimesheetDetails(MatchGuideConstants.TimesheetStatus.Cancelled);

            else if (segue.Identifier == "RejectedTimesheetsSelectedSegue")
                destinationController.LoadTimesheetDetails(MatchGuideConstants.TimesheetStatus.Rejected);

            base.PrepareForSegue(segue, sender);
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

