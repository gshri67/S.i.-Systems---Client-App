using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace App2
{
	internal class TimeEntryTableViewSource : UITableViewSource
	{
		private const string CellIdentifier = "cell";
		private UIViewController parentController;
        private List<String> clientNames;

		public TimeEntryTableViewSource( UIViewController parentController, List<String> clientNames ) 
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
			var cell = tableView.DequeueReusableCell(CellIdentifier);

            cell.TextLabel.Text = clientNames.ElementAt((int)indexPath.Item);
           

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