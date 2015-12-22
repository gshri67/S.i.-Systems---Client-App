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

			// Perform any additional setup after loading the view, typically from a nib.
		}

        public void LoadTimesheetSummary()
        {
            var task = _viewModel.LoadTimesheetSummary();

            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

	    private void UpdateUserInterface()
	    {
            OpenTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfOpenTimesheets;
            SubmittedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfSubmittedTimesheets;
            RejectedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfRejectedTimesheets;
            CancelledTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfCancelledTimesheets;   
	    }

	    public override void ViewDidAppear (bool animated)
		{
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
	}
}

