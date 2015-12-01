using System;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ContractorDetailsTableViewSource : UITableViewSource
    {
        private readonly ContractorDetailsTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController)
        {
            _parentController = parentController;
            //_parentModel = parentModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as ContractorContactInfoCell;

			cell.UpdateCell 
			(
				mainText: "Contact Info",
                subtitleText:"Subtitle",
                rightDetailText:"right detail"
			);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 3;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}