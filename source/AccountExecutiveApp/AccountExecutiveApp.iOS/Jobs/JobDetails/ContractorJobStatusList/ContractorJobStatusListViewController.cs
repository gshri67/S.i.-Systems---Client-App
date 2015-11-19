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

		public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
		}

	    public void LoadContractors(IEnumerable<Contractor> contractors)
	    {
	        _viewModel.LoadContractors(contractors);
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

            UpdateUserInterface();
        }
	}
}
