using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	internal class ActiveTimesheetTableViewSource : UITableViewSource
	{
		private const string CellIdentifier = "activeTimesheetCell";
        private readonly ActiveTimesheetViewController _parentController;

	    private readonly List<PayPeriod> _payPeriods;

        public ActiveTimesheetTableViewSource(ActiveTimesheetViewController parentController, IEnumerable<PayPeriod> payPeriods) 
		{
			this._parentController = parentController;

		    _payPeriods = payPeriods.OrderBy(pp=>pp.EndDate).ToList();
	    }

		public override nint NumberOfSections(UITableView tableView)
		{
			return _payPeriods.Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
		    return _payPeriods[(int)section] != null
                ? _payPeriods[(int)section].TimePeriod
                : "Unknown Pay Period";
		}

	    public override nint RowsInSection(UITableView tableview, nint section)
	    {
            return _payPeriods.ElementAt((int)section) != null
                ? _payPeriods.ElementAt((int)section).Timesheets.Count()
                : 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as ActiveTimesheetCell ??
                       new ActiveTimesheetCell(CellIdentifier);
		    var timesheet = _payPeriods.ElementAt(indexPath.Section).Timesheets.ElementAt(indexPath.Row);
            cell.UpdateCell(
                company: timesheet.ClientName,
                timesheetApprover: "Bob Smith",
                hours: timesheet.TimeEntries.Sum(t => t.Hours).ToString(),
                status: timesheet.Status.ToString()
            );

			return cell;
		}

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            _parentController.PerformSegue(ActiveTimesheetViewController.TimesheetSelectedSegue, indexPath);

            //normal iOS behaviour is to remove the selection
            tableView.DeselectRow(indexPath, true);
		}
	}
}

