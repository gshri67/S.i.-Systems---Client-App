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
			_contracts = contracts.ToList();

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
			
				string rightDetail = "Ends " + curContract.EndDate.ToString("MMM dd, yyyy");
				string subtitleText = "";

				if (curContract.IsFloThru)
					subtitleText = "FT";
				else
					subtitleText = "FS";

				cell.UpdateCell 
				(
					mainText:curContract.consultant.FullName,
					subtitleText:subtitleText,
					rightDetailText:rightDetail
				);
			}
				
			return cell;
		}
	}
}

