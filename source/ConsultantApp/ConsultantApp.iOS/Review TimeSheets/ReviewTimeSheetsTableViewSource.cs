using System;
using System.Collections.Generic;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS.Review_TimeSheets
{
	internal class ReviewTimeSheetsTableViewSource : UITableViewSource
	{
		private const string CellIdentifier = "reviewTimeSheetCell";
		private UIViewController parentController;

		public List<string> clientNames;
		public List<string> timePeriods;

		private TimesheetViewModel timesheetViewModel;

		public List<Timesheet> openTimesheets;
		public List<Timesheet> rejectedTimesheets;
		public List<Timesheet> pendingTimesheets;

		public ReviewTimeSheetsTableViewSource(UIViewController parentController) 
		{
			this.parentController = parentController;
            
            List<Timesheet> timesheets = new List<Timesheet>();

			openTimesheets = timesheets.Where(t=>t.Status.Equals(TimesheetStatus.Open)).ToList();
            rejectedTimesheets = timesheets.Where(t => t.Status.Equals(TimesheetStatus.Rejected)).ToList();
            pendingTimesheets = timesheets.Where(t => t.Status.Equals(TimesheetStatus.Pending)).ToList();
	    }

		public override nint NumberOfSections(UITableView tableView)
		{
			return 3;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (section == 0)
				return "Open";
			else if (section == 1)
				return "Rejected";
			else if (section == 2)
				return "Pending";
			else
				return null;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (section == 0)
				return openTimesheets.Count;
			else if (section == 1)
				return rejectedTimesheets.Count;
			else if (section == 2)
				return pendingTimesheets.Count;

			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			ReviewTimeSheetCell cell = (ReviewTimeSheetCell)tableView.DequeueReusableCell(CellIdentifier);

			Timesheet curSheet = null;

			if (indexPath.Section == 0)
				curSheet = openTimesheets.ElementAt((int)indexPath.Item);
			else if (indexPath.Section == 1)
				curSheet = rejectedTimesheets.ElementAt((int)indexPath.Item);
			else if (indexPath.Section == 2)
				curSheet = pendingTimesheets.ElementAt((int)indexPath.Item);

			if (curSheet == null)
				throw new NullReferenceException ( "Time sheet was null because it did not fall under the status of open, rejected, or pending." );

			cell.clientField.Text = curSheet.ClientName;
			cell.timePeriodField.Text = curSheet.TimePeriod;

			//cell.TextLabel.Text = "Timesheet";

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{/*
			UIStoryboard storyboard = parentController.Storyboard;
			CalendarTimeSheetViewController vc = (CalendarTimeSheetViewController)storyboard.InstantiateViewController( "CalendarTimeSheetViewController" );

			//UINavigationController NVC = parentController.NavigationController;
			parentController.NavigationController.PushViewController(vc, true);
*/
			//parentController.PresentViewController(vc, true, null);

			parentController.NavigationController.PushViewController ( parentController.Storyboard.InstantiateViewController("SubmitTimeSheetViewController"), true );
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			if (section == 0 && openTimesheets.Count == 0
				|| section == 1 && rejectedTimesheets.Count == 0
				|| section == 2 && pendingTimesheets.Count == 0)
					return 0;

			return 30;
		}
	}
}

