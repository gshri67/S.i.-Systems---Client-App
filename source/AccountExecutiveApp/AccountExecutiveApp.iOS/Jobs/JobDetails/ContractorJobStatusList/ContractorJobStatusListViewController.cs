using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	    public const string CellIdentifier = "ProposedContractorsTableViewCell";
		private SubtitleHeaderView _subtitleHeaderView;
		private string Subtitle;
        private JobStatus _status;

		public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
		}

        public void LoadContractorsWithJobIdAndStatusAndClientName( int Id, JobStatus status, string clientName )
        {
            _status = status;
            var task = _viewModel.LoadContractorsWithJobIDAndStatusAndClientName( Id, status, clientName );
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

			TableView.RegisterClassForCellReuse(typeof(ProposedContractorsTableViewCell), CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);


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
            //UpdateUserInterface();
        }


		private void UpdatePageTitle()
		{
			//if (_jobDetails != null) 
			{
				if (_status == JobStatus.Proposed)
					Title = "Proposed Contractors";
				else if (_status == JobStatus.Callout)
					Title = "Callouts";
				else
					Title = "Shortlisted Contractors";

			    Subtitle = _viewModel.ClientName;
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
