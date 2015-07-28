using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using ConsultantApp.SharedModels;

namespace App2
{
	internal class TimeEntryTableViewSource : UITableViewSource
	{
		//private const string CellIdentifier = "cell";
        private const string CellIdentifier = "TimeEntryCell";
		private UIViewController parentController;
        private List<TimeEntry> clientNames;

		public TimeEntryTableViewSource( UIViewController parentController, List<TimeEntry> clientNames ) 
		{
			this.parentController = parentController;
            /*
            clientNames = new NSMutableArray();
            clientNames.Add( new NSString("Nexen") );
            clientNames.Add( new NSString("Cenovus") );*/

            this.clientNames = clientNames;
           
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return clientNames.Count;
            //return clientNames.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			TimeEntryCell cell = (TimeEntryCell)tableView.DequeueReusableCell(CellIdentifier);

            //if (cell == null)
              //  cell = new TimeEntryCell();

            TimeEntry curEntry = clientNames.ElementAt((int)indexPath.Item);
            cell.TextLabel.Text = curEntry.clientName;

            cell.clientLabel.Text = curEntry.clientName;
            cell.projectCodeLabel.Text = curEntry.projectCode;
            cell.hoursLabel.Text = curEntry.hours.ToString();

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
}