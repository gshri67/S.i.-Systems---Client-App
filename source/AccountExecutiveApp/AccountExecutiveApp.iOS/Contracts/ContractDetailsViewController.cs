using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	partial class ContractDetailsViewController : UIViewController
	{
		private ConsultantContract _contract;
		public string subtitle;
		public int ContractID = -1;
	    private ContractsViewModel _contractsViewModel;

		public ContractDetailsViewController (IntPtr handle) : base (handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractsViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			LoadContract ();
		}

        private void SetupTableViewSource()
        {
            if (tableView == null || _contract == null)
                return;

            RegisterCellsForReuse();
            InstantiateTableViewSource();

            tableView.Source = new ContractDetailsTableViewSource(this, _contract);
            tableView.ReloadData();
        }

        private void RegisterCellsForReuse()
        {
            if (tableView == null) return;

            tableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
        }

        private void InstantiateTableViewSource()
        {
            tableView.Source = new ContractDetailsTableViewSource(this, _contract);
        }

	    public void UpdateSummaryView()
		{
			if (_contract == null)
				return;
			
			CompanyNameLabel.Text = _contract.CompanyName;
            PeriodLabel.Text = string.Format("{0} - {1}", _contract.StartDate.ToString("MMM dd, yyyy"), _contract.EndDate.ToString("MMM dd, yyyy"));

            BillRateLabel.Text = string.Format("${0}", _contract.BillRate.ToString("0.00"));
            PayRateLabel.Text = string.Format("${0}", _contract.PayRate.ToString("0.00"));
            GrossMarginLabel.Text = string.Format("${0}", _contract.GrossMargin.ToString("0.00"));
		}

		public async void LoadContract()
		{
		    _contract = await _contractsViewModel.GetContractWithId(ContractID);
			UpdateUI();
		}

	    private void UpdateUI()
	    {
            if (_contract != null && tableView != null && summaryView != null )
            {
                SetupTableViewSource();
                tableView.ReloadData();
                UpdateSummaryView();
            }
	    }
	}
}
