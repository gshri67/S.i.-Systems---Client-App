using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList;
using CoreGraphics;
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
        private LoadingOverlay _overlay;

        public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
		}

        public void LoadContractorsWithJobIdAndStatusAndClientNameAndTitle( int Id, JobStatus status, string clientName, string jobTitle )
        {
            IndicateLoading();
            _status = status;
            InvokeOnMainThread(UpdatePageTitle);

            _viewModel.JobTitle = jobTitle;
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
            InvokeOnMainThread(RemoveOverlay);
	    }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SearchManager.CreateNavBarRightButton(this);
            CreateCustomTitleBar();
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

                UpdateCustomTitleBar();
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
        private void UpdateCustomTitleBar()
        {
            if( _subtitleHeaderView != null )
            _subtitleHeaderView.TitleText = Title;
            _subtitleHeaderView.SubtitleText = Subtitle;
        }


        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);
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
