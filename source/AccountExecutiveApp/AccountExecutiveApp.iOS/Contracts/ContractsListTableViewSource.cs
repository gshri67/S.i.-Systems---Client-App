using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;

namespace AccountExecutiveApp.iOS
{
	public class ContractsListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

	    private ContractListTableViewModel _contractsTableModel;

        public ContractsListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContractSummary> contracts)
		{
			_parentController = parentVC;

            //Assuming there is always a contract
            _contractsTableModel = new ContractListTableViewModel( contracts );
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_contractsTableModel.HasContracts())
				return _contractsTableModel.NumberOfContracts();
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			string CellIdentifier = "SubtitleWithRightDetailCell";
			var cell = tableView.DequeueReusableCell (CellIdentifier) as SubtitleWithRightDetailCell;
            
			if (_contractsTableModel.HasContracts())
			{
                ConsultantContractSummary curContract = _contractsTableModel.ContractAtIndex((int)indexPath.Item);
			
				string subtitleText = "";
				string rightDetail = _contractsTableModel.DateDetailStringAtIndex((int)indexPath.Item);

				subtitleText = curContract.ClientName;

				cell.UpdateCell 
				(
					mainText:curContract.ContractorName,
					subtitleText:subtitleText,
					rightDetailText:rightDetail
				);
			}
				
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			ContractDetailsViewController vc = (ContractDetailsViewController)_parentController.Storyboard.InstantiateViewController ("ContractDetailsViewController");

            vc.LoadContract(_contractsTableModel.ContractIdByIndex((int)indexPath.Item));

			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}

