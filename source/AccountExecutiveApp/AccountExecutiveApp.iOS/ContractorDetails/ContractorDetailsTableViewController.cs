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
    public partial class ContractorDetailsTableViewController : UITableViewController
	{
	    private readonly ContractorDetailsViewModel _viewModel;
	    public const string CellIdentifier = "ContractorContactInfoCell";
        private int _id;
        private LoadingOverlay _overlay;

        public ContractorDetailsTableViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorDetailsViewModel>();
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
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            TableView.Source = new ContractorDetailsTableViewSource(this, _viewModel.Contractor);
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
            TableView.ReloadData();
        }

	    private void UpdateUserInterface()
	    {
	        InvokeOnMainThread(InstantiateTableViewSource);
            InvokeOnMainThread(UpdatePageTitle);
            InvokeOnMainThread(RemoveOverlay);
	    }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SearchManager.CreateNavBarRightButton(this);
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
