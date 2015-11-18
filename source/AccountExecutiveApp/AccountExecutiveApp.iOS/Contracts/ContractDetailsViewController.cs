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
		public string Subtitle;
		public int ContractID = -1;
	    private ContractDetailsViewModel _contractsViewModel;
        private SubtitleHeaderView _subtitleHeaderView;

		public ContractDetailsViewController (IntPtr handle) : base (handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractDetailsViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadContract ();
		}

        private void UpdatePageTitle()
        {
            if (_contract != null)
            {
                Title = string.Format("{0}", _contract.Title);
                Subtitle = string.Format("{0}", _contract.consultant.FullName);
            }
            else
            {
                Title = "Contract Overview";
                Subtitle = "";
            }
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
			
			CompanyNameLabel.Text = _contractsViewModel.CompanyNameString();
            PeriodLabel.Text = _contractsViewModel.DatePeriodString();

            BillRateLabel.Text = _contractsViewModel.BillRateString();
            PayRateLabel.Text = _contractsViewModel.PayRateString();
            GrossMarginLabel.Text = _contractsViewModel.GrossMarginString();
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
