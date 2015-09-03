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
		private const string expandedCellIdentifier = "AddProjectCodeCell";
		private UIViewController parentController;
		public IEnumerable<TimeEntry> _timeEntries;
		public IEnumerable<string> _projectCodes;
		public IEnumerable<string> _payRates;

		public delegate void TableDelegate(IEnumerable<TimeEntry> timeEntries);
		public  TableDelegate onDataChanged;

		private int normalCellHeight;
		private int expandedCellHeight;
		private int expandedCellIndex = -1;
		private int prevSelectedRow = -1;
		private bool addingProjectCode;//if there is an extra cell expanded for picker etc..

		public AddTimeTableViewSource( UIViewController parentController, IEnumerable<TimeEntry> timeEntries, IEnumerable<string> projectCodes, IEnumerable<string> payRates ) 
		{
			this.parentController = parentController;
			/*
            timeEntries = new NSMutableArray();
            timeEntries.Add( new NSString("Nexen") );
            timeEntries.Add( new NSString("Cenovus") );*/

			this._timeEntries = timeEntries;
			this._projectCodes = projectCodes;
			this._payRates = payRates;

			normalCellHeight = 44;
			expandedCellHeight = 200;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_timeEntries != null && _timeEntries.Any ()) 
			{
				Console.WriteLine ( "number of time entries " + _timeEntries.Count() );
				if (addingProjectCode) {		
					return _timeEntries.Count () + 1;
				} else 
				{
					return _timeEntries.Count ();
				}
			}
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			Console.WriteLine ( indexPath.Item + " " + expandedCellIndex );
			if ((int)indexPath.Item == expandedCellIndex) 
			{
				AddProjectCodeCell cell = (AddProjectCodeCell)tableView.DequeueReusableCell (expandedCellIdentifier);

				if (_timeEntries != null) 
				{
					TimeEntry curEntry = _timeEntries.ElementAt( prevSelectedRow );//((int)indexPath.Item);
					cell.setData ( curEntry, _projectCodes, _payRates );

                    if( cell.onSave == null )
					    cell.onSave += delegate
					    {
						    closeExpandedCell();

						    tableView.ReloadData();

							onDataChanged( _timeEntries);
					    };
                   
                    if( cell.onDelete == null )
					    cell.onDelete += delegate
					    {
						    Console.WriteLine(_timeEntries.Count());
						    closeExpandedCell();
						    Console.WriteLine("Finished Closing Expanded Cell");

						    Console.WriteLine("Prev selected row " + prevSelectedRow);

						    //if( prevSelectedRow >= 0 && prevSelectedRow < _timeEntries.Count() && 
                            var elem = _timeEntries.ElementAtOrDefault(prevSelectedRow);
                                if (elem != null)
                                {


							    Console.WriteLine(_timeEntries.Count());
							    var timeEntryToRemove = _timeEntries.ElementAtOrDefault(prevSelectedRow);
							    Console.WriteLine(timeEntryToRemove);
							    var timeEntriesList = _timeEntries.ToList();
							    timeEntriesList.Remove(timeEntryToRemove);

							    _timeEntries = timeEntriesList.AsEnumerable();
    //							Console.WriteLine(_timeEntries.ElementAt(prevSelectedRow));
    //							var elemToRemove = _timeEntries.ElementAt(prevSelectedRow);

    //							if( elemToRemove != null )
    //								_timeEntries = _timeEntries.Where( e => e != elemToRemove );
	
							    Console.WriteLine("Finished Removing the timeentry");
						    }
						Console.WriteLine(_timeEntries.Count());
						tableView.ReloadData();

						onDataChanged( _timeEntries);
					};
				}

				return cell;
			} else 
			{
				TimeEntryCell cell = (TimeEntryCell)tableView.DequeueReusableCell (CellIdentifier);
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
				if (_timeEntries != null && _timeEntries.Count () > entryIndex(indexPath)) {
					TimeEntry curEntry = _timeEntries.ElementAt (entryIndex (indexPath));//((int)indexPath.Item);

					cell.projectCodeField.Text = curEntry.ProjectCode;
					cell.payRateLabel.Text = curEntry.PayRate;
					cell.hoursField.Text = curEntry.Hours.ToString ();

					cell.onHoursChanged = ( float newHours) => {
						curEntry.Hours = newHours;
						onDataChanged( _timeEntries);
					};
				} else 
				{
					cell.projectCodeField.Text = "";
					cell.payRateLabel.Text = "";
					cell.hoursField.Text = "";
				}

				return cell;
			}
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			Console.WriteLine ( "selected: " + indexPath.Item + " " + expandedCellIndex );

			//Nothing happens if expanded cell is tapped
			if ((int)indexPath.Item != expandedCellIndex) {
				int realSelectedIndex = entryIndex (indexPath);

				if (realSelectedIndex == prevSelectedRow)
					addingProjectCode = !addingProjectCode;
				else
					addingProjectCode = true;
	
				if (addingProjectCode)
					expandedCellIndex = realSelectedIndex + 1;
				else
					expandedCellIndex = -1;

				tableView.ReloadData ();

				prevSelectedRow = realSelectedIndex;

				Console.WriteLine ("prev selected row: " + prevSelectedRow);
			}

			Console.WriteLine ("After row selected");
			//tableView.ReloadRows (tableView.IndexPathsForVisibleRows, UITableViewRowAnimation.Automatic);
			/*
			NSIndexPath[] paths = new NSIndexPath[]{ NSIndexPath.FromIndex((nuint)_timeEntries.Count() ) };
			GetCell (tableView, NSIndexPath.FromIndex ((nuint)_timeEntries.Count ())).Hidden = false;

			tableView.ReloadRows ( paths, UITableViewRowAnimation.Automatic);
*/
			//[tableView reloadRowsAtIndexPaths:[NSArray arrayWithObject:previousSelectedIndexPath]
			//	withRowAnimation:UITableViewRowAnimationAutomatic];
		}
			
		//if a cell was added
		public void handleNewCell()
		{
			if (!addingProjectCode)
				openExpandedCell ( _timeEntries.Count()-1 );
		}

		public void openExpandedCell( int index )
		{
			addingProjectCode = true;
			expandedCellIndex = index + 1;

			prevSelectedRow = index;

			//tableView.ReloadData();
		}

		public void closeExpandedCell()
		{
			expandedCellIndex = -1;
			addingProjectCode = false;

			//tableView.ReloadData();
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if( (int)indexPath.Item != expandedCellIndex )
				return normalCellHeight;
			return expandedCellHeight;
			/*			
			 if ([expandedCells containsObject:indexPath])
			{
				return kExpandedCellHeight; //It's not necessary a constant, though
			}
			else
			{
				return kNormalCellHeigh; //Again not necessary a constant
			}
			*/	
		}

		//return the time entry at the index
		public int entryIndex (NSIndexPath indexPath )
		{
			//there is no expanded cell or the index is before the expanded cell
			if (!addingProjectCode || expandedCellIndex > (int)indexPath.Item )
				return (int)indexPath.Item;
			else if( expandedCellIndex == (int)indexPath.Item )//this function should not be called on this index, but we return -1 for safety
				return -1;
			else //if the cell is after the expanded cell
				return (int)indexPath.Item-1;
		}
	}
}
