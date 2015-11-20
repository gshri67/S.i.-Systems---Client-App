using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public partial class ContractorJobStatusListViewController : UITableViewController
	{
	    private readonly ContractorJobStatusListViewModel _viewModel;
	    public const string CellIdentifier = "CandidateCell";
		private SubtitleHeaderView _subtitleHeaderView;
		private string Subtitle;
		private JobDetails _jobDetails; //move to view model

		public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
		}

	    public void LoadContractors(IEnumerable<Contractor> contractors)
	    {
	        _viewModel.LoadContractors(contractors);
	    }
		public void LoadJobDetails( JobDetails JobDetails )
		{
			_jobDetails = JobDetails;
		}

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), CellIdentifier);

            TableView.Source = new ContractorCandidateTableViewSource(this, _viewModel);

            TableView.ReloadData();
        }

	    private void UpdateUserInterface()
	    {
	        InvokeOnMainThread(InstantiateTableViewSource);
	    }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			UpdatePageTitle ();
			CreateCustomTitleBar ();
            UpdateUserInterface();
        }


		private void UpdatePageTitle()
		{
			if (_jobDetails != null) 
			{
				if (_jobDetails.JobStatus == JobStatus.Proposed)
					Title = "Proposed Contractors";
				else if (_jobDetails.JobStatus == JobStatus.Callout)
					Title = "Callouts";
				else
					Title = "Shortlisted Contractors";
				
				Subtitle = _jobDetails.ClientName + " - " + _jobDetails.Title;
			}
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
	}
}
