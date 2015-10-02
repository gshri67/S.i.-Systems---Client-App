using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS
{
	public class AddTimeTableViewSource : UITableViewSource
	{

		private const string CellIdentifier = "TimeEntryCell";
		private const string ExpandedCellIdentifier = "AddProjectCodeCell";
		public IEnumerable<TimeEntry> TimeEntries;
		public IEnumerable<string> ProjectCodes;
		public IEnumerable<PayRate> PayRates;

		public delegate void TableDelegate(IEnumerable<TimeEntry> timeEntries);
		public  TableDelegate OnDataChanged;

		private readonly int _normalCellHeight;
		private readonly int _expandedCellHeight;
		private int _expandedCellIndex = -1;
		private int _prevSelectedRow = -1;
		private bool _addingProjectCode;//if there is an extra cell expanded for picker etc..
		private bool _isEnabled = true;

		//This functionality is likely going to be removed
		public bool MustSave;//if this is true, the done or delete button must be tapped before the cell can be minimized.
		//this is true either the first time it is created, or if something has been editted in the cell but changes have not been saved

		public AddTimeTableViewSource( IEnumerable<TimeEntry> timeEntries, IEnumerable<string> projectCodes, IEnumerable<PayRate> payRates ) 
		{
			this.TimeEntries = timeEntries;
			this.ProjectCodes = projectCodes;
			this.PayRates = payRates;

			_normalCellHeight = 44;
			_expandedCellHeight = 200;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
		    if (TimeEntries == null || !TimeEntries.Any()) return 0;

		    if (_addingProjectCode) {		
		        return TimeEntries.Count () + 1;
		    }
		    return TimeEntries.Count ();
		}

	    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			if ((int)indexPath.Item == _expandedCellIndex)
				return getExpandedCell (tableView, indexPath);
			else
				return getTimeEntryCell ( tableView, indexPath );
		}

		private TimeEntryCell getTimeEntryCell( UITableView tableView, NSIndexPath indexPath )
		{
			var cell = (TimeEntryCell)tableView.DequeueReusableCell (CellIdentifier);

			if (TimeEntries != null && TimeEntries.Count () > EntryIndex(indexPath)) 
			{
				var curEntry = TimeEntries.ElementAt(EntryIndex(indexPath));
				populateTimeEntryCell ( cell, curEntry );
			} 
			else 
				populateTimeEntryCell (cell, null);

			cell.enable (_isEnabled);

			return cell;
		}

		private AddProjectCodeCell getExpandedCell( UITableView tableView, NSIndexPath indexPath )
		{
			var cell = (AddProjectCodeCell)tableView.DequeueReusableCell (ExpandedCellIdentifier);

			if (TimeEntries == null) return cell;

			var curEntry = TimeEntries.ElementAt( _prevSelectedRow );//((int)indexPath.Item);
			cell.SetData( curEntry, ProjectCodes, PayRates );

			if (cell.OnSave == null)
				setupExpansionCellSave (cell, tableView);

			if (cell.OnDelete == null)
				setupExpansionCellDelete (cell, tableView);

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			//Nothing happens if expanded cell is tapped
		    if ((int) indexPath.Item == _expandedCellIndex || MustSave) return;

		    var realSelectedIndex = EntryIndex (indexPath);

		    if (realSelectedIndex == _prevSelectedRow)
		        _addingProjectCode = !_addingProjectCode;
		    else
		        _addingProjectCode = true;
	
		    if (_addingProjectCode)
		        _expandedCellIndex = realSelectedIndex + 1;
		    else 
		    {
		        var expandedCell = (AddProjectCodeCell)tableView.CellAt ( NSIndexPath.FromItemSection(_expandedCellIndex, 0) );
		        expandedCell.SaveChanges ();
		        _expandedCellIndex = -1;
		    }

		    tableView.ReloadData ();
			_prevSelectedRow = realSelectedIndex;

			//if (_addingProjectCode)
				//scrollToExpandedCell (tableView);
		}
			
		//if a cell was added
		public void HandleNewCell()
		{
			if (!_addingProjectCode) 
			{
				OpenExpandedCell (TimeEntries.Count () - 1);

			}
		}

		public void OpenExpandedCell( int index )
		{
			_addingProjectCode = true;
			_expandedCellIndex = index + 1;

			_prevSelectedRow = index;
		}

		public void CloseExpandedCell()
		{
			_expandedCellIndex = -1;
			_addingProjectCode = false;
		}

		public void saveOpenExpandedCells( UITableView tableView )
		{
			if (_addingProjectCode == true && _expandedCellIndex >= 0 ) 
			{
				AddProjectCodeCell cell = (AddProjectCodeCell)tableView.CellAt ( NSIndexPath.FromItemSection(_expandedCellIndex, 0) );

				cell.SaveChanges ();
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
		    return (int)indexPath.Item != _expandedCellIndex 
                ? _normalCellHeight 
                : _expandedCellHeight;
		}

	    //return the time entry at the index
		public int EntryIndex (NSIndexPath indexPath )
		{
		    //there is no expanded cell or the index is before the expanded cell
			if (!_addingProjectCode || _expandedCellIndex > (int)indexPath.Item )
				return (int)indexPath.Item;

		    if( _expandedCellIndex == (int)indexPath.Item )//this function should not be called on this index, but we return -1 for safety
		        return -1;

		    return (int)indexPath.Item-1;
		}

	    public void Enable( bool shouldEnable )
		{
			_isEnabled = shouldEnable;
		}

		public void scrollToExpandedCell( UITableView tableview )
		{
			Console.WriteLine ("prev selected index " + _prevSelectedRow);

			tableview.ScrollToRow (NSIndexPath.FromItemSection (_prevSelectedRow, 0), UITableViewScrollPosition.Top, false);
		}

		//project code cell is used for the expansion details
		#region ExpansionCell 

		private void setupExpansionCellSave( AddProjectCodeCell cell, UITableView tableView )
		{
			cell.OnSave += delegate
			{
				CloseExpandedCell();

				tableView.ReloadData();

				OnDataChanged( TimeEntries);

				MustSave = false;
			};
		}
		private void setupExpansionCellDelete( AddProjectCodeCell cell, UITableView tableView )
		{
			cell.OnDelete += delegate
			{
				CloseExpandedCell();

				var elem = TimeEntries.ElementAtOrDefault(_prevSelectedRow);
				if (elem != null)
				{
					var timeEntryToRemove = TimeEntries.ElementAtOrDefault(_prevSelectedRow);
					var timeEntriesList = TimeEntries.ToList();
					timeEntriesList.Remove(timeEntryToRemove);

					TimeEntries = timeEntriesList.AsEnumerable();

					MustSave = false;
				}

				tableView.ReloadData();

				OnDataChanged(TimeEntries);
			};
		}

		#endregion

		//TimeEntryCells
		#region EntryCells 

		private void populateTimeEntryCell( TimeEntryCell cell, TimeEntry curEntry )
		{
			if (curEntry != null) 
			{
				cell.UpdateCell
				(
					projectCode: curEntry.ProjectCode,
					rateDescription: curEntry.PayRate.RateDescription,
					hours: curEntry.Hours.ToString (CultureInfo.InvariantCulture),
					onHoursChanged: ( float newHours) => 
									{
										curEntry.Hours = newHours;
										OnDataChanged (TimeEntries);
									}
				);
			} 
			else 
			{
				cell.UpdateCell
				(
					projectCode: null,
					rateDescription: null,
					hours: null,
					onHoursChanged: null
				);
			}
		}

		#endregion
	}
}
