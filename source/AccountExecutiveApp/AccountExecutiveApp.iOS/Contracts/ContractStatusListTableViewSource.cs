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

        private Dictionary<ContractType, Dictionary<ContractStatusType, List<ConsultantContract>>> ContractsGroupedByTypeAndStatus; 

        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
        {
            PopulateContractsDictionaryByTypeAndStatus(contracts);

            _parentController = parentVC;
        }

	    private void PopulateContractsDictionaryByTypeAndStatus(IEnumerable<ConsultantContract> contracts )
	    {
	        var fsDict = DictionaryWithContractsByStatus(contracts, ContractType.FullySourced);
            var ftDict = DictionaryWithContractsByStatus(contracts, ContractType.FloThru);

	        ContractsGroupedByTypeAndStatus =
	            new Dictionary<ContractType, Dictionary<ContractStatusType, List<ConsultantContract>>>();
	        ContractsGroupedByTypeAndStatus[ContractType.FullySourced] = fsDict;
	        ContractsGroupedByTypeAndStatus[ContractType.FloThru] = ftDict;
	    }

        private static Dictionary<ContractStatusType, List<ConsultantContract>> DictionaryWithContractsByStatus(IEnumerable<ConsultantContract> contracts, ContractType type)
	    {
	        Dictionary<ContractStatusType, List<ConsultantContract>> fsDict =
	            new Dictionary<ContractStatusType, List<ConsultantContract>>();

            List<ConsultantContract> endingContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Ending).ToList();
            List<ConsultantContract> startingContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Starting).ToList();
            List<ConsultantContract> activeContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Active).ToList();

            if (endingContracts.Count > 0)
                fsDict[ContractStatusType.Ending] = endingContracts;
            if (startingContracts.Count > 0)
                fsDict[ContractStatusType.Starting] =startingContracts;
            if (activeContracts.Count > 0)
                fsDict[ContractStatusType.Active] = activeContracts;

	        return fsDict;
	    }

	    public void swapInContractList( List<List<ConsultantContract>> list, int i, int j ) 
        {
            List<ConsultantContract> temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            if (ContractsGroupedByTypeAndStatus!= null )
    			return ContractsGroupedByTypeAndStatus.Keys.Count;
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
            if (section == 0 && ContractsGroupedByTypeAndStatus != null)
                return ContractsGroupedByTypeAndStatus[ContractType.FullySourced].Count();
            else if (section == 1 && ContractsGroupedByTypeAndStatus != null )
                return ContractsGroupedByTypeAndStatus[ContractType.FloThru].Count();
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
	        if (ContractsGroupedByTypeAndStatus != null )//(!_fullySourcedContractsGroupedByTypeAndStatus.Any())
	            return indexPath.Section == 0
                ? ContractsGroupedByTypeAndStatus[ContractType.FullySourced].Values.ElementAt((int)indexPath.Item)
                : ContractsGroupedByTypeAndStatus[ContractType.FloThru].Values.ElementAt((int)indexPath.Item);

            else
	        {
	            return null;//_floThruContractsGroupedByTypeAndStatus.ElementAtOrDefault((int)indexPath.Item).ToList();
            }
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

	        List<ConsultantContract> contractsByStatus = ContractsGroupedByTypeAndStatus.Values.ElementAt((int)indexPath.Section).Values.ElementAt((int)indexPath.Item);

            vc.setContracts( contractsByStatus );
            vc.Title = string.Format("{0} Contracts", contractsByStatus[0].StatusType.ToString());
            vc.Subtitle = string.Format("{0} Contracts", contractsByStatus[0].ContractType.ToString());
     
            _parentController.ShowViewController ( vc, _parentController );
		}
	}
}


