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
		private const string CellIdentifier = "reviewTimeSheetCell";
        private readonly ActiveTimesheetViewController _parentController;

	    private readonly List<IGrouping<string, Timesheet>> _timesheets;

        public ActiveTimesheetTableViewSource(ActiveTimesheetViewController parentController, IEnumerable<Timesheet> timesheets) 
		{
			this._parentController = parentController;

		    _timesheets = timesheets.GroupBy(timesheet => timesheet.TimePeriod).ToList();
	    }

		public override nint NumberOfSections(UITableView tableView)
		{
			return _timesheets.Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
		    return _timesheets.ElementAt((int) section) != null 
                ? _timesheets.ElementAt((int) section).Key 
                : "Unknown Pay Period";
		}

	    public override nint RowsInSection(UITableView tableview, nint section)
	    {
			return _timesheets.ElementAt((int) section) != null
                ? _timesheets.ElementAt((int) section).Count()
                : 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as ActiveTimesheetCell ??
                       new ActiveTimesheetCell(CellIdentifier);

		    SetCellLabels(_timesheets[indexPath.Section].ElementAt(indexPath.Row), cell);

			return cell;
		}

	    private void SetCellLabels(Timesheet timesheet, ActiveTimesheetCell cell)
	    {
            cell.UpdateCell(
                company: timesheet.ClientName, 
                timesheetApprover: "Bob Smith", 
                hours: timesheet.TimeEntries.Sum(t=>t.Hours).ToString(), 
                status: timesheet.Status.ToString()
            );
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			_parentController.NavigationController.PushViewController ( _parentController.Storyboard.InstantiateViewController("SubmitTimeSheetViewController"), true );
		}
	}
}

