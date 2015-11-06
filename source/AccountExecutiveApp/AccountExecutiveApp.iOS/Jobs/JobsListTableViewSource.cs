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
        private const string CellIdentifier = "SubtitleWithRightDetailCell";

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
            var cell = tableView.DequeueReusableCell (CellIdentifier) as SubtitleWithRightDetailCell ?? new SubtitleWithRightDetailCell(CellIdentifier);

		    if (_jobs == null) return cell;

		    Job curJob = _jobs [(int)indexPath.Item];

		    cell.UpdateCell 
		    (
		        mainText:curJob.JobTitle,
		        subtitleText:SubtitleText(curJob),
		        rightDetailText:RightDetailText(curJob)
		    );

		    return cell;
		}

	    private string SubtitleText(Job curJob)
	    {
	        DateTime curDate = DateTime.Now;
	        int daysSinceStart = curDate.Subtract(curJob.issueDate).Days;

	        
	        return TimePassed(daysSinceStart);
	    }

	    private string TimePassed( int daysSinceStart )
	    {
            string subtitleText = "New";

            if (daysSinceStart == 1)
                subtitleText = "1 day ago";
            else if (daysSinceStart < 7)
                subtitleText = daysSinceStart.ToString() + " days ago";
            else if (daysSinceStart < 14)
                subtitleText = "1 week ago";
            else if (daysSinceStart < 30)
                subtitleText = (daysSinceStart / 7).ToString() + " weeks ago";
            else if (daysSinceStart < 60)
                subtitleText = "1 month ago";
            else if (daysSinceStart >= 60)
                subtitleText = (daysSinceStart / 30).ToString() + " months ago";

	        return subtitleText;
	    }

	    private string RightDetailText(Job curJob)
	    {
	        string rightDetail = "";
	        if (curJob.hasCallout)
	            rightDetail = "Callout";
	        else if (curJob.isProposed)
	            rightDetail = "Proposed";
	        return rightDetail;
	    }
	}
}

