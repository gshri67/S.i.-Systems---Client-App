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

            _payPeriods = payPeriods.OrderByDescending(pp => pp.EndDate).ToList();
	    }

		public override nint NumberOfSections(UITableView tableView)
		{
			if (_payPeriods != null && _payPeriods.Count() > 0 )
				return _payPeriods.Count;
			else
				return 0;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (_payPeriods == null || _payPeriods.Count() == 0 )
				return "";

		    return _payPeriods[(int)section] != null
                ? _payPeriods[(int)section].TimePeriod
                : "Unknown Pay Period";
		}

	    public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_payPeriods == null || _payPeriods.Count () == 0)
				return 0;
			
            return _payPeriods.ElementAt((int)section) != null
                ? _payPeriods.ElementAt((int)section).Timesheets.Count()
                : 0;
		}

	    private static string StatusTextToDisplay(MatchGuideConstants.TimesheetStatus status)
	    {
            if (status == MatchGuideConstants.TimesheetStatus.Batched
                || status == MatchGuideConstants.TimesheetStatus.Moved
                || status == MatchGuideConstants.TimesheetStatus.Accepted)
            {
                return MatchGuideConstants.TimesheetStatus.Approved.ToString();
            }
	        return status.ToString();
	    }

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as ActiveTimesheetCell ??
                       new ActiveTimesheetCell(CellIdentifier);
		    var timesheet = _payPeriods.ElementAt(indexPath.Section).Timesheets.ElementAt(indexPath.Row);

            cell.UpdateCell(
                company: timesheet.ClientName,
                timesheetApprover: timesheet.TimesheetApprover.Email,
                hours: timesheet.TimeEntries.Sum(t => t.Hours).ToString(),
                status: StatusTextToDisplay(timesheet.Status)
            );

			return cell;
		}

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            _parentController.PerformSegue(ActiveTimesheetViewController.TimesheetSelectedSegue, indexPath);

            //normal iOS behaviour is to remove the selection
            tableView.DeselectRow(indexPath, true);
		}

        public Timesheet GetItem(NSIndexPath indexPath)
        {
            return _payPeriods.ElementAt(indexPath.Section).Timesheets.ElementAt(indexPath.Row);
        }


		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 50;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			if (_payPeriods == null || _payPeriods.Count () == 0)
				return 0;
	
			return 32;
		}

		public override UIView GetViewForFooter (UITableView tableView, nint section)
		{
			if (NumberOfSections(tableView) <= 1 || section == NumberOfSections (tableView) - 1) 
			{
				UILabel footerView = new UILabel ();
				footerView.Text = "Please use the Desktop portal to view Timesheets older than 6 months";
				footerView.Lines = 0;
				footerView.BackgroundColor = UIColor.Clear;
				footerView.TextAlignment = UITextAlignment.Center;

				return footerView;
			}
			return null;
		}

		public override nfloat GetHeightForFooter (UITableView tableView, nint section)
		{
			if ( NumberOfSections(tableView) <= 1 || section == NumberOfSections (tableView) - 1) 
				return 150;
			return 0;
		}
	}
}

