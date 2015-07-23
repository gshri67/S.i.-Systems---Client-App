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
		private UIViewController parentController;

        public TimeSheetsOverviewTableViewSource(TimeSheetsOverviewViewController parentController) 
        {
			this.parentController = parentController;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if (section == 0)
                return "Active";
            else if (section == 1)
                return "Submitted for Approval";
            else
                return "";
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (section == 0)
                return 3;
            else if( section == 1)
                return 1;

            return 1;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier);

            cell.TextLabel.Text = "Timesheet";

            return cell;
        }

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			UIStoryboard storyboard = parentController.Storyboard;
			CalendarTimeSheetViewController vc = (CalendarTimeSheetViewController)storyboard.InstantiateViewController( "CalendarTimeSheetViewController" );
			parentController.PresentViewController(vc, true, null);

		}
    }
}