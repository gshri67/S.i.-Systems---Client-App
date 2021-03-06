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
	public partial class ContractHistoryTableViewController : Si_TableViewController
	{
		public const string CellIdentifier = "SubtitleWithRightDetailCell";
		private IEnumerable<ConsultantContract> Contracts;
		private string Subtitle;

		public ContractHistoryTableViewController(IntPtr handle)
			: base(handle) { }

		public ContractHistoryTableViewController()
		{
			TableView = new UITableView( View.Frame, UITableViewStyle.Grouped );
		}

		public void setContracts(IEnumerable<ConsultantContract> contracts)
		{
			Contracts = contracts;
			UpdatePageTitle();
			UpdateUserInterface();
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null)
				return;

			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), CellIdentifier);

			TableView.Source = new ContractHistoryTableViewSource(this, Contracts);
			TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);
			TableView.ReloadData();
		}

		private void UpdateUserInterface()
		{
			if (TableView != null && TableView.Source == null && Contracts != null)
				InvokeOnMainThread(InstantiateTableViewSource);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if( ShowSearchIcon )
	            SearchManager.CreateNavBarRightButton(this);

			UpdateUserInterface();
		}

		private void UpdatePageTitle()
		{
			Title = "Contracts";

			if (Contracts.Any())
				Subtitle = Contracts.FirstOrDefault().ContractorName;
			else
				Subtitle = string.Empty;
		}
	}
}

