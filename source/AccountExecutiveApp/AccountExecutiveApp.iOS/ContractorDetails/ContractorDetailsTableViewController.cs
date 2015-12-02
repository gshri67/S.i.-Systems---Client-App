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
        public int Id;

		public ContractorDetailsTableViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorDetailsViewModel>();
		}
    /*
        public void LoadContractorsWithJobIdAndStatusAndClientName( int Id, JobStatus status, string clientName )
        {
            _status = status;
            var task = _viewModel.LoadContractorsWithJobIDAndStatusAndClientName( Id, status, clientName );
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }*/

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

			TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleWithRightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);

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

            LoadContractor();
        }


		private void UpdatePageTitle()
		{
		    Title = _viewModel.PageTitle;
		}


        public async void LoadContractor()
        {
            var task = _viewModel.LoadContractor(Id);
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
	}
}
