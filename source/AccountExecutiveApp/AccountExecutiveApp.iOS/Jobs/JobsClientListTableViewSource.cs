using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;

namespace AccountExecutiveApp.iOS
{
    public class JobsClientListTableViewSource : UITableViewSource
    {
        private readonly JobsClientListViewController _parentController;
        private readonly JobsClientListTableViewModel _listViewModel;

        public JobsClientListTableViewSource(JobsClientListViewController parentViewController, IEnumerable<JobSummary> jobs)
        {
            _listViewModel = new JobsClientListTableViewModel(jobs);

            _parentController = parentViewController;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _listViewModel.NumberOfGroups();
        }

		private void SetCellTextByRowNumber(RightDetailCell cell, int rowNumber)
        {
			cell.UpdateCell (
				mainText: _listViewModel.ClientNameByRowNumber(rowNumber),
				rightDetailText: _listViewModel.JobStateCountsByRowNumber(rowNumber)
			);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(JobsClientListViewController.CellReuseIdentifier) as RightDetailCell;

            SetCellTextByRowNumber(cell, (int)indexPath.Item);

            return cell;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            if (_listViewModel.JobSummaries.Any())
                return null;

            var label = new UILabel
            {
                Text = "You have no jobs available at this time.",
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(14),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };
            tableView.Add(label);
            return label;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var viewController = (JobsListViewController)_parentController.Storyboard.InstantiateViewController("JobsListViewController");

            viewController.SetClientID(_listViewModel.ClientIDByRowNumber((int)indexPath.Item));
            viewController.Subtitle = _listViewModel.ClientNameByRowNumber((int)indexPath.Item);
            _parentController.ShowViewController(viewController, _parentController);
        }
    }
}

