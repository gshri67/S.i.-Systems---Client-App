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
    public partial class ContractorDetailsTableViewController : UITableViewController
	{
	    private readonly ContractorDetailsViewModel _viewModel;
	    public const string CellIdentifier = "ContractorContactInfoCell";
        private int _id;

		public ContractorDetailsTableViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorDetailsViewModel>();
		}

        public void setContractorId(int Id)
        {
            _id = Id;
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
	    }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			UpdatePageTitle ();
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
	}
}
