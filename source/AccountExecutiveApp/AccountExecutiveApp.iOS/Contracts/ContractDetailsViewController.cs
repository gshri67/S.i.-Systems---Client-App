using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountExecutiveApp.iOS
{
	partial class ContractDetailsViewController : UIViewController
	{
		private readonly ContractDetailsViewModel _viewModel;
		public int ContractID = -1;
	    private ContractDetailsViewModel _contractsViewModel;
        private SubtitleHeaderView _subtitleHeaderView;

		public void LoadContract(int id)
	    {
	        var task = _viewModel.LoadContractDetails(id);
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    private void UpdateUserInterface()
	    {
            SetupTableViewSource();
            UpdateSummaryView();
		}

        private void UpdatePageTitle()
        {
            if (_contractsViewModel.HasContract())
            {
                Title = _contractsViewModel.Title();
                Subtitle = _contractsViewModel.ConsultantFullNameString();
            }
            else
            {
                Title = "Contract Overview";
                Subtitle = "";
            }
        }


		public ContractDetailsViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<ContractDetailsViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

        private void SetupTableViewSource()
        {
            if (tableView == null)
                return;

            tableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");

            tableView.Source = new ContractDetailsTableViewSource(this, _viewModel.Contract);
            tableView.ReloadData();
        }

	    public void UpdateSummaryView()
		{
			CompanyNameLabel.Text = _viewModel.CompanyName;
            PeriodLabel.Text = _viewModel.ContractPeriod;

			CompanyNameLabel.Text = _contractsViewModel.CompanyNameString();
            PeriodLabel.Text = _contractsViewModel.DatePeriodString();
            BillRateLabel.Text = _viewModel.BillRate;
            PayRateLabel.Text = _viewModel.PayRate;
            GrossMarginLabel.Text = _viewModel.GrossMargin;
		}

		public async void LoadContract()
		{
		    await _contractsViewModel.GetContractWithId(ContractID);
			UpdateUI();
		}

	    private void UpdateUI()
	    {
            if (_contractsViewModel.HasContract() && tableView != null && summaryView != null )
            {
                SetupTableViewSource();
                tableView.ReloadData();
                UpdateSummaryView();

                UpdatePageTitle();
                CreateCustomTitleBar();
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
