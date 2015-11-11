using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.ViewModel;

namespace AccountExecutiveApp.iOS
{
	public class JobsListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

		private readonly JobsListViewModel _jobsListViewModel;

		public JobsListTableViewSource ( UITableViewController parentViewController, JobsListViewModel jobs )
		{
			_jobsListViewModel = jobs;
		}


		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return _jobsListViewModel.Jobs.Count();
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            var cell = tableView.DequeueReusableCell (JobsListViewController.SubtitleCellIdentifier) as SubtitleWithRightDetailCell;

            var rowNumber = (int)indexPath.Item;

            cell.UpdateCell
            (
                mainText: _jobsListViewModel.JobTitleByIndex(rowNumber),
                subtitleText: _jobsListViewModel.TimeDescriptionByIndex(rowNumber),
                rightDetailText: _jobsListViewModel.JobStatusByIndex(rowNumber)
            );

		    return cell;
		}
	}
}

