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

        public JobsClientListTableViewSource(JobsClientListViewController parentViewController, IEnumerable<Job> jobs)
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

        private void SetCellTextByRowNumber(UITableViewCell cell, int rowNumber)
        {
            cell.TextLabel.Text = _listViewModel.ClientNameByRowNumber(rowNumber);

            if (cell.DetailTextLabel != null)
                cell.DetailTextLabel.Text = _listViewModel.JobStateCountsByRowNumber(rowNumber);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(JobsClientListViewController.CellReuseIdentifier) as RightDetailCell;

            SetCellTextByRowNumber(cell, (int)indexPath.Item);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var viewController = (JobsListViewController)_parentController.Storyboard.InstantiateViewController("JobsListViewController");

            viewController.SetJobs(_listViewModel.JobsByRowNumber((int)indexPath.Item));

            _parentController.ShowViewController(viewController, _parentController);
        }
    }
}

