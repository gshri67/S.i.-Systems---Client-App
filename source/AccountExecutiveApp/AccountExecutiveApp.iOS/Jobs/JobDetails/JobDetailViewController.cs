using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class JobDetailViewController : UITableViewController
	{
        private readonly NSObject _tokenExpiredObserver;
        private LoadingOverlay _overlay;
	    private readonly JobDetailViewModel _viewModel;

	    private const string ShortlistedSegueIdentifier = "ShortlistedSegue";
        private const string ProposedSegueIdentifier = "ProposedSegue";
        private const string CalloutSegueIdentifier = "CalloutSegue";

		public JobDetailViewController (IntPtr handle) : base (handle)
		{
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
            _viewModel = DependencyResolver.Current.Resolve<JobDetailViewModel>();
		}

        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
        }

	    public void LoadJob(Job job)
	    {
	        var task = _viewModel.LoadJob(job);

            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    private void UpdateUserInterface()
	    {
	        InstantiateTableViewSource();
            RemoveOverlay();
	    }

        private void InstantiateTableViewSource()
        {
            ClientContactName.Text = _viewModel.ClientContactName;
            DirectReportName.Text = _viewModel.DirectReportName;
            ShortListed.TextLabel.Text = "Shortlisted";
            ShortListed.DetailTextLabel.Text = _viewModel.NumberOfShortlistedConsultants.ToString();
            Proposed.TextLabel.Text = "Proposed";
            Proposed.DetailTextLabel.Text = _viewModel.NumberOfProposedContractors.ToString();
            Callouts.TextLabel.Text = "Callouts";
            Callouts.DetailTextLabel.Text = _viewModel.NumberOfContractorsWithCallouts.ToString();
            
            TableView.ReloadData();
        }

	    public override string TitleForHeader(UITableView tableView, nint section)
	    {
	        if (section == 0)
	        {
	            return _viewModel.JobTitle;
	        }
	        return base.TitleForHeader(tableView, section);
	    }

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
            IndicateLoading();
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
	        base.PrepareForSegue(segue, sender);

	        if (segue.Identifier == ShortlistedSegueIdentifier)
	        {
                LoadConsultantsIntoDestinationController(segue, _viewModel.ShortlistedConsultants);
	        }
	        else if (segue.Identifier == ProposedSegueIdentifier)
	        {
                LoadConsultantsIntoDestinationController(segue, _viewModel.ProposedConsultants);
            }else if (segue.Identifier == CalloutSegueIdentifier)
            {
                LoadConsultantsIntoDestinationController(segue, _viewModel.ConsultantsWithCallouts);
            }
	    }

	    private void LoadConsultantsIntoDestinationController(UIStoryboardSegue segue, IEnumerable<IM_Consultant> consultants)
	    {
	        var destinationController = segue.DestinationViewController as ContractorJobStatusListViewController;

	        if (destinationController != null)
	            destinationController.LoadConsultants(consultants);
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
