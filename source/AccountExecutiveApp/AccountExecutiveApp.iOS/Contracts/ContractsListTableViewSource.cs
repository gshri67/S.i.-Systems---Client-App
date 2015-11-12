using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class ContractsListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

		private List<ConsultantContract> _contracts;

        public ContractsListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
		{
			_parentController = parentVC;
			_contracts = contracts.ToList();
            SortContracts();
		}

	    private void SortContracts()
	    {
	        if (_contracts != null)
	        {
                if( _contracts[0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Starting )
                    SortContractsByStartDate();
                else
                    SortContractsByEndDate();
	        }
	    }

	    private void SortContractsByStartDate()
        {
            _contracts.Sort((d1, d2) => DateTime.Compare(d1.StartDate, d2.StartDate));
        }
        private void SortContractsByEndDate()
        {
            _contracts.Sort((d1, d2) => DateTime.Compare(d1.EndDate, d2.EndDate));
        }

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_contracts != null)
				return _contracts.Count();
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			//SubtitleWithRightDetailCell cell = (SubtitleWithRightDetailCell)tableView.DequeueReusableCell ("SubtitleWithRightDetailCell");

			string CellIdentifier = "SubtitleWithRightDetailCell";
			var cell = tableView.DequeueReusableCell (CellIdentifier) as SubtitleWithRightDetailCell;
			//??	new SubtitleWithRightDetailCell(CellIdentifier);
            
			if (_contracts != null)
			{
				ConsultantContract curContract = _contracts [(int)indexPath.Item];
			
				string rightDetail;
				string subtitleText = "";

				if( curContract.StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Starting )
					rightDetail = "Starts " + curContract.StartDate.ToString("MMM dd, yyyy");
				else
					rightDetail = "Ends " + curContract.EndDate.ToString("MMM dd, yyyy");

				subtitleText = curContract.CompanyName;

				cell.UpdateCell 
				(
					mainText:curContract.consultant.FullName,
					subtitleText:subtitleText,
					rightDetailText:rightDetail
				);
			}
				
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			ContractDetailsViewController vc = (ContractDetailsViewController)_parentController.Storyboard.InstantiateViewController ("ContractDetailsViewController");
			/*
			if (indexPath.Section == 0 && FS_contractsByStatus != null) {
				vc.setContracts (FS_contractsByStatus [(int)indexPath.Item]);
				vc.Title = string.Format ("{0} Contracts", FS_contractsByStatus [(int)indexPath.Item] [0].StatusType);
				vc.subtitle = "Fully-Sourced";
			}*/

			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}

