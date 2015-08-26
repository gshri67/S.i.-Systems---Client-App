using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS
{
	public class AddTimeTableViewSource : UITableViewSource
	{

		private const string CellIdentifier = "TimeEntryCell";
		private UIViewController parentController;
		public IEnumerable<TimeEntry> _timeEntries;

		public AddTimeTableViewSource( UIViewController parentController, IEnumerable<TimeEntry> timeEntries ) 
		{
			this.parentController = parentController;
			/*
            timeEntries = new NSMutableArray();
            timeEntries.Add( new NSString("Nexen") );
            timeEntries.Add( new NSString("Cenovus") );*/

			this._timeEntries = timeEntries;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_timeEntries != null && _timeEntries.Any())
				return _timeEntries.Count();
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			TimeEntryCell cell = (TimeEntryCell)tableView.DequeueReusableCell(CellIdentifier);
			/*
			//if (cell == null)
			//  cell = new TimeEntryCell();

			if (_timeEntries != null && _timeEntries.Any())
			{
				TimeEntry curEntry = _timeEntries.ElementAt((int)indexPath.Item);

				cell.TextLabel.Text = curEntry.ClientName;

				cell.clientField.Text = curEntry.ClientName;
				cell.projectCodeField.Text = curEntry.ProjectCode;
				cell.hoursField.Text = curEntry.Hours.ToString();

				cell.onHoursChanged = ( int newHours) => {
					curEntry.Hours = newHours;
				};

				cell.onClientChanged = ( String newClient) => {
					UIPickerView picker = new UIPickerView ();
					picker.Model = new TimeEntryClientPickerModel (parentController);
					//cell.clientField.InputView = picker;
					//parentController.View.AddSubview( picker );
				};
			}*/

			if (_timeEntries != null && _timeEntries.Count () > (int)indexPath.Item) {
				TimeEntry curEntry = _timeEntries.ElementAt ((int)indexPath.Item);

				cell.projectCodeField.Text = curEntry.ProjectCode;
				cell.hoursField.Text = curEntry.Hours.ToString ();
			} 

			return cell;
		}

	}
}
