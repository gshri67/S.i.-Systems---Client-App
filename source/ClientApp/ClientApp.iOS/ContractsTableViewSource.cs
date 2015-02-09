using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    class ContractsTableViewSource : UITableViewSource
    {
        private readonly List<ConsultantGroup> _consultantGroups;

        private const string CellIdentifier = "ConsultantGroupCell";

        private readonly ContractorViewController _parentController;

        public ContractsTableViewSource(ContractorViewController parentController, IEnumerable<ConsultantGroup> consultantGroups)
        {
            _parentController = parentController;

            _consultantGroups = consultantGroups.ToList();
        }

        private string BuildDetailText(ConsultantGroup consultantGroup)
        {
            var description = string.Format("{0} Alumni", consultantGroup.Consultants.Count);

            return description;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) ??
                       new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);

            SetCellLabels(indexPath, cell);

            return cell;
        }

        private void SetCellLabels(NSIndexPath indexPath, UITableViewCell cell)
        {
            //assign the visual aspects of the cell
            cell.TextLabel.Text = _consultantGroups[indexPath.Row].Specialization;
            cell.DetailTextLabel.Text = BuildDetailText(_consultantGroups[indexPath.Row]);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _consultantGroups.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //normal iOS behaviour is to remove the selection
            tableView.DeselectRow(indexPath, true);

            _parentController.PerformSegue("ConsultantGroupSelected", indexPath);
        }
    }
}