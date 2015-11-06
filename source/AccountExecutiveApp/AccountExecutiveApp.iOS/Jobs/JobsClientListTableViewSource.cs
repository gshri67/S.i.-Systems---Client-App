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
        private readonly UITableViewController _parentController;
        private readonly JobsClientListTableViewModel _listViewModel;

        public JobsClientListTableViewSource(UITableViewController parentViewController, IEnumerable<Job> jobs)
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

        private RightDetailCell GetRightDetailCellFromTableView(UITableView tableView)
        {
            const string rightCellIdendtifier = "RightDetailCell";
            return tableView.DequeueReusableCell(rightCellIdendtifier) as RightDetailCell
                ?? new RightDetailCell(UITableViewCellStyle.Value1, rightCellIdendtifier);
        }

        private void SetCellTextByRowNumber(UITableViewCell cell, int rowNumber)
        {
            cell.TextLabel.Text = _listViewModel.ClientNameByRowNumber(rowNumber);

            if (cell.DetailTextLabel != null)
                cell.DetailTextLabel.Text = _listViewModel.JobStateCountsByRowNumber(rowNumber);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = GetRightDetailCellFromTableView(tableView);

            SetCellTextByRowNumber(cell, (int)indexPath.Item);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            JobsListViewController vc = (JobsListViewController)_parentController.Storyboard.InstantiateViewController("JobsListViewController");
            vc.setJobs(_listViewModel.JobsByRowNumber((int)indexPath.Item));
            _parentController.ShowViewController(vc, _parentController);
        }
    }
}

