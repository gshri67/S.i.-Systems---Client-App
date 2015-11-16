using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList
{
    public class ContractorCandidateTableViewSource : UITableViewSource
    {
        private readonly ContractorJobStatusListViewController _parentController;
        private readonly ContractorJobStatusListViewModel _parentModel;
        
        public ContractorCandidateTableViewSource(ContractorJobStatusListViewController parentController, ContractorJobStatusListViewModel parentModel)
        {
            _parentController = parentController;
            _parentModel = parentModel;
        }

        private void SetCellTextByRowNumber(UITableViewCell cell, int rowNumber)
        {
            cell.TextLabel.Text = _parentModel.ContractorNameByRowNumber(rowNumber);

            if (cell.DetailTextLabel != null)
                cell.DetailTextLabel.Text = _parentModel.ContractorStatusByRowNumber(rowNumber);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(ContractorJobStatusListViewController.CellIdentifier) as RightDetailCell;

            SetCellTextByRowNumber(cell, (int)indexPath.Item);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _parentModel.NumberOfContractors();
        }
    }
}