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

		public TimeEntryTableViewSource( UIViewController parentController) 
		{
			this.parentController = parentController;
            /*
            clientNames = new NSMutableArray();
            clientNames.Add( new NSString("Nexen") );
            clientNames.Add( new NSString("Cenovus") );*/

            clientNames = new List<String>();
            clientNames.Add("Nexen");
            clientNames.Add("Cenovus");
           
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			//return (nint)clientNames.Count;
            return clientNames.Count;
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
        
        public void addTimeEntry( String clientName ) 
        {
            /*
            String[] newClients = new String[clientNames.Length + 1];

            for (int i = 0; i < clientNames.Length; i++)
                newClients[i] = clientNames[i];

            newClients[clientNames.Length] = clientName;*/

            clientNames.Add("new client");
        }
	}
}