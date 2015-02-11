using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    public class DisciplineTableViewSource : UITableViewSource
    {
        private DisciplineViewController _parentController;
        private readonly ConsultantGroup _consultantGroup;
        private const string CellIdentifier = "DisciplineCell";

        public DisciplineTableViewSource(DisciplineViewController parentController, ConsultantGroup consultantGroups)
        {
            _parentController = parentController;

            _consultantGroup = consultantGroups;
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
            cell.TextLabel.Text = _consultantGroup.Consultants[indexPath.Row].FullName;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _consultantGroup.Consultants.Count;
        }
    }
}