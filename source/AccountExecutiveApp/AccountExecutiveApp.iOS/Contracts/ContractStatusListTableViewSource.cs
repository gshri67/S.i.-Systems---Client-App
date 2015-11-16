using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;

namespace AccountExecutiveApp.iOS
{
	public class ContractStatusListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;
	    private ContractStatusListTableViewModel _contractsTableModel;

        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
        {
            _contractsTableModel = new ContractStatusListTableViewModel( contracts );

            _parentController = parentVC;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            if ( _contractsTableModel.HasContracts() )
    			return _contractsTableModel.NumberOfContractTypes();
		    return 1;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (section == 0)
				return "Fully-Sourced";
			else
				return "Flo-Thru";
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            if (section == 0 && _contractsTableModel.HasContracts() )
                return _contractsTableModel.NumberOfStatusesWithContractsOfType( ContractType.FullySourced );
            else if (section == 1 && _contractsTableModel.HasContracts())
                return _contractsTableModel.NumberOfStatusesWithContractsOfType(ContractType.FloThru);
            else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			var contractsByStatus = GetContractsByStatusAndSection(indexPath);
    
	        UpdateCellText(cell, contractsByStatus);

		    return cell;
		}

	    private static void UpdateCellText(UITableViewCell cell, List<ConsultantContract> contractsByStatus)
	    {
	        if (contractsByStatus == null)
	            return; 

            if( contractsByStatus.Count > 0 )
	            cell.TextLabel.Text = contractsByStatus[0].StatusType.ToString();

	        if (cell.DetailTextLabel != null)
	            cell.DetailTextLabel.Text = contractsByStatus.Count().ToString();
	    }

	    private List<ConsultantContract> GetContractsByStatusAndSection(NSIndexPath indexPath)
	    {
	        if ( _contractsTableModel.HasContracts() )
	            return indexPath.Section == 0
                ? _contractsTableModel.contractsWithTypeAndStatusIndex(ContractType.FullySourced, (int)indexPath.Item)
                : _contractsTableModel.contractsWithTypeAndStatusIndex(ContractType.FloThru, (int)indexPath.Item);

            return null;
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

	        List<ConsultantContract> contractsByStatus = _contractsTableModel.contractsWithTypeIndexAndStatusIndex((int) indexPath.Section, (int) indexPath.Item);

            vc.setContracts( contractsByStatus );
            vc.Title = string.Format("{0} Contracts", contractsByStatus[0].StatusType.ToString());
            vc.Subtitle = string.Format("{0} Contracts", contractsByStatus[0].ContractType.ToString());
     
            _parentController.ShowViewController ( vc, _parentController );
		}
	}
}


