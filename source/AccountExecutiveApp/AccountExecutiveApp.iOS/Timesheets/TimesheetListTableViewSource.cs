using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;

namespace AccountExecutiveApp.iOS
{
	public class TimesheetListTableViewSource : UITableViewSource
	{
		private readonly TimesheetListTableViewController _parentController;
		private readonly TimesheetListTableViewModel _listViewModel;

		public TimesheetListTableViewSource(TimesheetListTableViewController parentViewController)
		{
			_listViewModel = new TimesheetListTableViewModel(null);

			_parentController = parentViewController;
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
			var cell = tableView.DequeueReusableCell(SubtitleWithRightDetailCell.CellIdentifier) as SubtitleWithRightDetailCell;

			return cell;
		}
		/*
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var viewController = (JobsListViewController)_parentController.Storyboard.InstantiateViewController("JobsListViewController");

			viewController.SetClientID(_listViewModel.ClientIDByRowNumber((int)indexPath.Item));
			viewController.Subtitle = _listViewModel.ClientNameByRowNumber((int)indexPath.Item);
			_parentController.ShowViewController(viewController, _parentController);
		}*/
	}
}

