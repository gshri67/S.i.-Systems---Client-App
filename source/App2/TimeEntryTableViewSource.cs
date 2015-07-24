﻿using System;
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

		public TimeEntryTableViewSource( UIViewController parentController) 
		{
			this.parentController = parentController;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 2;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			var cell = tableView.DequeueReusableCell(CellIdentifier);

			cell.TextLabel.Text = "Timesheet";

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