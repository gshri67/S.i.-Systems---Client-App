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
	partial class JobDetailViewController : UITableViewController
	{
        private readonly NSObject _tokenExpiredObserver;
        private LoadingOverlay _overlay;
	    private readonly JobDetailViewModel _viewModel;

	    private const string ShortlistedSegueIdentifier = "ShortlistedSegue";
        private const string ProposedSegueIdentifier = "ProposedSegue";
        private const string CalloutSegueIdentifier = "CalloutSegue";
		private const string ClientContactSegueIdentifier = "ClientContactSegue";

		private string Subtitle;
		private SubtitleHeaderView _subtitleHeaderView;
	    private bool _needsCreateTitleBar = false;

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

	    public void LoadJobWithJobId( int Id )
	    {
            IndicateLoading();

	        var task = _viewModel.LoadJobWithJobID(Id);

            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    private void UpdateUserInterface()
	    {
	        InstantiateTableViewSource();
            InvokeOnMainThread(RemoveOverlay);

			UpdatePageTitle ();
            InvokeOnMainThread(CreateTitleBarIfNeeded);
	    }

        private void InstantiateTableViewSource()
        {
            ClientContactName.Text = _viewModel.ClientContactName;
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

            SearchManager.CreateNavBarRightButton(this);
	    }

	    public override void ViewDidAppear(bool animated)
        {
            CreateTitleBarIfNeeded();
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
	        base.PrepareForSegue(segue, sender);

			if (segue.Identifier == ClientContactSegueIdentifier) 
			{
				var destinationController = segue.DestinationViewController as ClientContactDetailsViewController;
				destinationController.SetContactId( _viewModel.ClientContactId, UserContactType.ClientContact);
			}
	        if (segue.Identifier == ShortlistedSegueIdentifier)
                LoadContractorsIntoDestinationController(segue, JobStatus.Shortlisted);
	        else if (segue.Identifier == ProposedSegueIdentifier)
                LoadContractorsIntoDestinationController(segue, JobStatus.Proposed);
            else if (segue.Identifier == CalloutSegueIdentifier)
                LoadContractorsIntoDestinationController(segue, JobStatus.Callout);
	    }

	    private void LoadContractorsIntoDestinationController(UIStoryboardSegue segue, JobStatus status)//IEnumerable<Contractor> consultants)
	    {
	        var destinationController = segue.DestinationViewController as ContractorJobStatusListViewController;

			if (destinationController != null) 
			{
				//destinationController.LoadContractors (consultants);
				//destinationController.LoadContractorStatusDetails (_viewModel.Job.Id, status);
                destinationController.LoadContractorsWithJobIdAndStatusAndClientNameAndTitle( _viewModel.Job.Id, status, _viewModel.ClientName, _viewModel.JobTitle);
			}
	    }

		private void UpdatePageTitle()
		{
			Title = "Job Details";
			Subtitle = string.Format("{0}", _viewModel.ClientName );
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

	    public void SetJobId(int id)
	    {
	        LoadJobWithJobId(id);
	    }


	}
}
