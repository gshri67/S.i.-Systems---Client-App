using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class JobsListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

		private List<Job> _jobs;

		public JobsListTableViewSource ( UITableViewController parentVC, IEnumerable<Job> jobs )
		{
			_jobs = jobs.ToList();

		}


		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_jobs != null)
				return _jobs.Count();
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			if (cell == null)
			{
				Console.WriteLine ("creating new cell");

				cell = new RightDetailCell (UITableViewCellStyle.Value1, "RightDetailCell");
			}

			if (_jobs != null)
			{
				Job curJob = _jobs [(int)indexPath.Item];
				cell.TextLabel.Text = curJob.JobTitle;

				if (cell.DetailTextLabel != null)
				{
					if (curJob.hasCallout)
						cell.DetailTextLabel.Text = "Callout";
					else if (curJob.isProposed)
						cell.DetailTextLabel.Text = "Proposed";
					
				}
			}


			return cell;
		}
	}
}

