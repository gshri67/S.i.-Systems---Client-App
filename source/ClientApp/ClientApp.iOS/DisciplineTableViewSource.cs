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
            var cell = tableView.DequeueReusableCell(CellIdentifier) as CustomDisciplineCell ??
                       new CustomDisciplineCell(CellIdentifier);

            var consultant = _consultantGroup.Consultants[indexPath.Row];
            var contractDate = string.Format("{0} - {1}", consultant.MostRecentContractStartDate,
                consultant.MostRecentContractEndDate);



            cell.UpdateCell(consultant.FullName, contractDate, consultant.MostRecentContractRate.ToString(), consultant.Rating);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _consultantGroup.Consultants.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _parentController.PerformSegue("ConsultantSelected", this);
        }

        public ConsultantSummary GetItem(int row)
        {
            return _consultantGroup.Consultants[row];
        }
    }
}