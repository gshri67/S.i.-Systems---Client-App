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
			//SubtitleWithRightDetailCell cell = (SubtitleWithRightDetailCell)tableView.DequeueReusableCell ("SubtitleWithRightDetailCell");

			string CellIdentifier = "SubtitleWithRightDetailCell";
			var cell = tableView.DequeueReusableCell (CellIdentifier) as SubtitleWithRightDetailCell;
			//??	new SubtitleWithRightDetailCell(CellIdentifier);

			if (_jobs != null)
			{
				Job curJob = _jobs [(int)indexPath.Item];

				string rightDetail = "";
				if (curJob.hasCallout)
					rightDetail = "Callout";
				else if (curJob.isProposed)
					rightDetail = "Proposed";

				cell.UpdateCell 
				(
					mainText:curJob.JobTitle,
					subtitleText:curJob.ClientName,
					rightDetailText:rightDetail
				);
			}


			return cell;
		}
	}
}

