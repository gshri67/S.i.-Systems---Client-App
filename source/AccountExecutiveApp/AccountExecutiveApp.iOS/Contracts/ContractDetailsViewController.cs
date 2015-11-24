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
            CreateCustomTitleBar();
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
            tableView.ScrollEnabled = false;
			tableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
            tableView.Source = new ContractDetailsTableViewSource(this, _viewModel.Contract);
            tableView.ReloadData();
        }

	    public void UpdateSummaryView()
	    {
	        if (summaryView == null) return;

            PeriodLabel.Text = _viewModel.ContractPeriod;
            BillRateLabel.Text = _viewModel.FormattedBillRate;
            PayRateLabel.Text = _viewModel.FormattedPayRate;
            GrossMarginLabel.Text = _viewModel.FormattedGrossMargin;
			MarkupLabel.Text = _viewModel.FormattedMarkup;

			ContractTitleLabel.Text = _viewModel.ContractTitle;
			ClientAndStatusLabel.Text = _viewModel.FormattedClientAndStatus;
		}

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
					/*
					string title = _viewModel.ContractTitle;
					if( title.Length > 25 )
						title = title.Substring(0,25);
*/
				_subtitleHeaderView.TitleText = "Contract Details";

				//if( _viewModel != null && _viewModel.CompanyName != null )
				//	_subtitleHeaderView.SubtitleText = _viewModel.CompanyName;

				_subtitleHeaderView.SubtitleText = "";

				NavigationItem.Title = "";
            });
        }
	}
}
