using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
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
        private JobStatus _status;

		public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
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
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
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
				if (_status == JobStatus.Proposed)
					Title = "Proposed Contractors";
				else if (_status == JobStatus.Callout)
					Title = "Callouts";
				else
					Title = "Shortlisted Contractors";

			    Subtitle = _jobDetails.ClientName;
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

        public void LoadContractorStatusDetails(JobDetails jobDetails, JobStatus status)
        {
            if( status == JobStatus.Shortlisted )
                _viewModel.LoadContractors(jobDetails.Shortlisted);
            else if( status == JobStatus.Proposed )
                _viewModel.LoadContractors(jobDetails.Proposed);
            else if (status == JobStatus.Callout)
                _viewModel.LoadContractors(jobDetails.Callouts);
            else
                _viewModel.LoadContractors( (new List<Contractor>()).AsEnumerable() );

            _jobDetails = jobDetails;
            _status = status;
        }
	}
}
