using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace App2
{
    internal class TimeSheetsOverviewTableViewSource : UITableViewSource
    {
        private const string CellIdentifier = "cell";

        public TimeSheetsOverviewTableViewSource(TimeSheetsOverviewViewController parentController) 
        { 
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 5;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier);

            cell.TextLabel.Text = "Timesheet";

            return cell;
        }
    }
}