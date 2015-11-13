using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class ContractStatusListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

	    private readonly IEnumerable<IGrouping<ContractType, ConsultantContract>> _fullySourcedContractsGroupedByTypeAndStatus;
        private readonly IEnumerable<IGrouping<ContractType, ConsultantContract>> _floThruContractsGroupedByTypeAndStatus;
	
        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
		{
            //_fullySourcedContractsGroupedByTypeAndStatus = contracts.Where(contract => contract.IsFullySourced)
            //                                            .GroupBy(contract => contract.StatusType)
            //                                            .OrderBy(grouping => grouping.Key);
            //_floThruContractsGroupedByTypeAndStatus = contracts.Where(contract => contract.IsFloThru)
            //                                            .GroupBy(contract => contract.StatusType)
            //                                            .OrderBy(grouping => grouping.Key);

            _parentController = parentVC;
		}

	    public void swapInContractList( List<List<ConsultantContract>> list, int i, int j ) 
        {
            List<ConsultantContract> temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
			return 2;
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
            //if ( section == 0 && FS_contractsByStatus != null)
            //    return FS_contractsByStatus.Count();
            //else if ( section == 1 && FT_contractsByStatus != null)
            //    return FT_contractsByStatus.Count();
            //else
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

	        cell.TextLabel.Text = contractsByStatus[0].StatusType.ToString();

	        if (cell.DetailTextLabel != null)
	            cell.DetailTextLabel.Text = contractsByStatus.Count().ToString();
	    }

	    private List<ConsultantContract> GetContractsByStatusAndSection(NSIndexPath indexPath)
	    {
            if (!_fullySourcedContractsGroupedByTypeAndStatus.Any())
	        return indexPath.Section == 0 
                ? _fullySourcedContractsGroupedByTypeAndStatus.ElementAtOrDefault((int) indexPath.Item).ToList() 
                : _floThruContractsGroupedByTypeAndStatus.ElementAtOrDefault((int)indexPath.Item).ToList();

            else
            {
                return _floThruContractsGroupedByTypeAndStatus.ElementAtOrDefault((int)indexPath.Item).ToList();
            }
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            //ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

            //if (indexPath.Section == 0 && FS_contractsByStatus != null) {
            //    vc.setContracts (FS_contractsByStatus [(int)indexPath.Item]);
            //    vc.Title = string.Format ("{0} Contracts", FS_contractsByStatus [(int)indexPath.Item] [0].StatusType);
                //vc.Subtitle = "Fully-Sourced";
            //}
            //else if (indexPath.Section == 1 && FT_contractsByStatus != null) 
            //{
            //    vc.setContracts (FT_contractsByStatus [(int)indexPath.Item]);
            //    vc.Title = string.Format ("{0} Contracts", FT_contractsByStatus [(int)indexPath.Item][0].StatusType);
                //vc.Subtitle = "Flo-Thru";
            //}
            //_parentController.ShowViewController ( vc, _parentController );
		}
	}
}


