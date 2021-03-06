﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	internal class TimesheetOverviewTableViewSource : UITableViewSource
	{
		//private const string CellIdentifier = "cell";
        private const string CellIdentifier = "TimeEntryCell";
		private UIViewController parentController;
        private readonly IEnumerable<TimeEntry> _timeEntries;

		public TimesheetOverviewTableViewSource( UIViewController parentController, IEnumerable<TimeEntry> timeEntries ) 
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
				return 1;
			//return timeEntries.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			TimeEntryCell cell = (TimeEntryCell)tableView.DequeueReusableCell(CellIdentifier);

            //if (cell == null)
              //  cell = new TimeEntryCell();

            if (_timeEntries != null && _timeEntries.Any())
            {
                TimeEntry curEntry = _timeEntries.ElementAt((int)indexPath.Item);

            cell.TextLabel.Text = curEntry.ClientName;

            cell.clientField.Text = curEntry.ClientName;
            cell.projectCodeField.Text = curEntry.ProjectCode;
            cell.hoursField.Text = curEntry.Hours.ToString();

				cell.onHoursChanged = ( float newHours) => {
                curEntry.Hours = newHours;
				};

				cell.onClientChanged = ( String newClient) => {
					UIPickerView picker = new UIPickerView ();
					picker.Model = new TimeEntryClientPickerModel (parentController);
					//cell.clientField.InputView = picker;
					//parentController.View.AddSubview( picker );
				};
			}

            return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{/*
			UIStoryboard storyboard = parentController.Storyboard;
			CalendarTimeSheetViewController vc = (CalendarTimeSheetViewController)storyboard.InstantiateViewController( "CalendarTimeSheetViewController" );

			//UINavigationController NVC = parentController.NavigationController;
			//parentController.NavigationController.PushViewController(vc, true);

			parentController.PresentViewController(vc, true, null);*/
		}
	}

    public class TimeEntryClientPickerModel : UIPickerViewModel
    {
        static string[] names = new string[] {
				"Brian Kernighan",
				"Dennis Ritchie",
				"Ken Thompson",
				"Kirk McKusick",
				"Rob Pike",
				"Dave Presotto",
				"Steve Johnson"
			};

        UIViewController pvc;
        public TimeEntryClientPickerModel(UIViewController pvc)
        {
            this.pvc = pvc;
        }

        public override nint GetComponentCount(UIPickerView v)
        {
            return 2;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return names.Length;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            if (component == 0)
                return names[row];
            else
                return row.ToString();
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            /*
            pvc.label.Text = String.Format("{0} - {1}",
                            names[picker.SelectedRowInComponent(0)],
                            picker.SelectedRowInComponent(1));*/
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            if (component == 0)
                return 240f;
            else
                return 40f;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }
    }
}