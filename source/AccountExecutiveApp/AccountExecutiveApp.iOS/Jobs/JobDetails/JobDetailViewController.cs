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

		private string Subtitle;
		private SubtitleHeaderView _subtitleHeaderView;

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

			UpdatePageTitle ();
			CreateCustomTitleBar ();
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
                LoadContractorsIntoDestinationController(segue, _viewModel.ShortlistedContractors);
	        }
	        else if (segue.Identifier == ProposedSegueIdentifier)
	        {
                LoadContractorsIntoDestinationController(segue, _viewModel.ProposedContractors);
            }else if (segue.Identifier == CalloutSegueIdentifier)
            {
                LoadContractorsIntoDestinationController(segue, _viewModel.ContractorsWithCallouts);
            }
	    }

	    private void LoadContractorsIntoDestinationController(UIStoryboardSegue segue, IEnumerable<Contractor> consultants)
	    {
	        var destinationController = segue.DestinationViewController as ContractorJobStatusListViewController;

	        if (destinationController != null)
	            destinationController.LoadContractors(consultants);
	    }

		private void UpdatePageTitle()
		{
			Title = "Job Details";
			Subtitle = string.Format("{0}", _viewModel.ClientName );
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
	}
}
