using System;
using UIKit;
using Foundation;

namespace AccountExecutiveApp.iOS
{
	public class JobsClientListTableViewSource : UITableViewSource
	{
		public JobsClientListTableViewSource ()
		{
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 5;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			if (cell == null)
			{
				Console.WriteLine ("creating new cell");

				cell = new RightDetailCell (UITableViewCellStyle.Value1, "RightDetailCell");
			}

			cell.TextLabel.Text = "cell";

			if( cell.DetailTextLabel != null )
				cell.DetailTextLabel.Text = "18/11/3";

			return cell;
		}

	}
}

