using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList;
using CoreFoundation;
using CoreGraphics;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class ContractorDetailsTableViewController : Si_TableViewController
	{
	    private readonly ContractorDetailsViewModel _viewModel;
	    public const string CellIdentifier = "ContractorContactInfoCell";
        private int _id;
        private string _jobDescription;
	    private bool _jobDescriptionWasSet = false;
	    public int JobId = -1;

        private LoadingOverlay _overlay;

        public ContractorDetailsTableViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorDetailsViewModel>();
		}

	    public void SetupWithContractorAndJobInformation( int Id, string jobDescription, int jobId )
	    {
	        _jobDescription = jobDescription;
	        _jobDescriptionWasSet = true;
            setContractorId(Id);
	        JobId = jobId;
	    }

	    public void setContractorId(int Id)
        {
            _id = Id;
            IndicateLoading();
            LoadContractor();
        }

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

			TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleWithRightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(LinkedInSearchCell), LinkedInSearchCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            if( _jobDescriptionWasSet )
                TableView.Source = new ContractorDetailsTableViewSource(this, _viewModel.Contractor, _jobDescription);
            else
                TableView.Source = new ContractorDetailsTableViewSource(this, _viewModel.Contractor);

            TableView.ReloadData();
        }

	    private void UpdateUserInterface()
	    {
	        InvokeOnMainThread(InstantiateTableViewSource);
            InvokeOnMainThread(UpdatePageTitle);
            InvokeOnMainThread(RemoveOverlay);
            InvokeOnMainThread(StopRefreshing);
        }

        public void StopRefreshing()
        {
            if (RefreshControl != null && RefreshControl.Refreshing)
                RefreshControl.EndRefreshing();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			if( ShowSearchIcon )
	            SearchManager.CreateNavBarRightButton(this);

			EdgesForExtendedLayout = UIRectEdge.None;
			ExtendedLayoutIncludesOpaqueBars = false;
			AutomaticallyAdjustsScrollViewInsets = false;

            TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);

            RefreshControl = new UIRefreshControl();
            RefreshControl.Bounds = new CGRect(RefreshControl.Bounds.X, RefreshControl.Bounds.Y - 35, RefreshControl.Bounds.Width, RefreshControl.Bounds.Height);
            RefreshControl.ValueChanged += delegate
            {
                if (_overlay != null)
                    _overlay.Hidden = true;

                LoadContractor();
            };
        }

	    public override void ViewWillAppear(bool animated)
	    {
            if (_jobDescriptionWasSet && _viewModel.ContractIdFromJobIdAndContractorId(JobId, _id) > 0 )//contractor is no longer shortlisted, but is in fact placed
	        {
                InvokeOnMainThread(() =>
                {
                    NavigationController.PopViewController(false);//we don't want to see this job details page again

                    int contractId = 1;

                    var vc = (ContractDetailsViewController) Storyboard.InstantiateViewController("ContractDetailsViewController");
                    vc.LoadContract(contractId);
                    ShowViewController( vc, this );
                });
              
	        }
	    }

	    private void UpdatePageTitle()
		{
		    Title = _viewModel.PageTitle;
		}

        public async void LoadContractor()
        {
            var task = _viewModel.LoadContractor(_id);
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
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
