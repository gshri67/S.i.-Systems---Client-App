using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    internal class AlumniTableViewSource : UITableViewSource
    {
        private readonly List<ConsultantGroup> _consultantGroups;

        private const string CellIdentifier = "ConsultantGroupCell";

        private readonly AlumniViewController _parentController;

        public AlumniTableViewSource(AlumniViewController parentController,
            IEnumerable<ConsultantGroup> consultantGroups)
        {
            _parentController = parentController;

            _consultantGroups = consultantGroups.ToList();
        }

        private string BuildDetailText(ConsultantGroup consultantGroup)
        {
            var description = string.Format("{0}", consultantGroup.Consultants.Count);

            return description;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as CustomAlumniCell ??
                       new CustomAlumniCell(CellIdentifier);

            SetCellLabels(indexPath, cell);

            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        private void SetCellLabels(NSIndexPath indexPath, CustomAlumniCell cell)
        {
            var specialization = string.IsNullOrEmpty(_consultantGroups[indexPath.Row].Specialization) ? "No Specialization" : _consultantGroups[indexPath.Row].Specialization;
            var numAlumni = BuildDetailText(_consultantGroups[indexPath.Row]);
            cell.UpdateCell(specialization, numAlumni);
        }

        private string BuildFooterText()
        {
            var alumniCount = _consultantGroups.Sum(x => x.Consultants.Count);
            var specializationsCount = _consultantGroups.Count();

            return specializationsCount == 0
                ? string.Format("No Results")
                : string.Format("Total: {0} {2}, {1} Specializations", alumniCount, specializationsCount,
                    _parentController.IsAlumniSelected ? "Alumni" : "Active");
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var label = new UILabel(new CGRect(0, 0, tableView.Frame.Width, 30));
            label.Text = BuildFooterText();
            label.TextAlignment = UITextAlignment.Center;
            label.Font = UIFont.SystemFontOfSize(14);
            label.BackgroundColor = UIColor.White;
            tableView.Add(label);
            return label;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _consultantGroups.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _parentController.PerformSegue("DisciplineSelected", indexPath);

            //normal iOS behaviour is to remove the selection
            tableView.DeselectRow(indexPath, true);
        }

        public ConsultantGroup GetItem(int row)
        {
            return _consultantGroups[(int)row];
        }
    }
}