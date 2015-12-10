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
	    private bool _needsCreateTitleBar = false;

	    public void LoadContract(int id)
	    {
	        var task = _viewModel.LoadContractDetails(id);
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    private void UpdateUserInterface()
	    {
            SetupTableViewSource();
            UpdateSummaryView();
            InvokeOnMainThread(CreateTitleBarIfNeeded);
		}

		public ContractDetailsViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<ContractDetailsViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		    summaryView.Hidden = true;
		}

	    public override void ViewDidAppear(bool animated)
	    {
            InvokeOnMainThread(CreateTitleBarIfNeeded);
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

            summaryView.Hidden = false;

	        ContractTitleLabel.Text = _viewModel.ContractTitle;

			PeriodLabel.Text = string.Format("{0} {1} {2}", _viewModel.FormattedStartDate, StyleGuideConstants.DateSeperator ,_viewModel.FormattedEndDate);
            BillRateLabel.Text = _viewModel.FormattedBillRate;
            PayRateLabel.Text = _viewModel.FormattedPayRate;
            GrossMarginLabel.Text = _viewModel.FormattedGrossMargin;
			MarginLabel.Text = _viewModel.FormattedMargin;
			MarkupLabel.Text = _viewModel.FormattedMarkup;

			ClientAndStatusLabel.Text = _viewModel.FormattedClientAndStatus;
		}

        private void CreateTitleBarIfNeeded()
        {
            if (!_needsCreateTitleBar)
                _needsCreateTitleBar = true;
            else
                InvokeOnMainThread(CreateCustomTitleBar);
        }

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;

				_subtitleHeaderView.TitleText = "Contract Details";
				_subtitleHeaderView.SubtitleText = "";

				NavigationItem.Title = "";
            });
        }
	}
}
