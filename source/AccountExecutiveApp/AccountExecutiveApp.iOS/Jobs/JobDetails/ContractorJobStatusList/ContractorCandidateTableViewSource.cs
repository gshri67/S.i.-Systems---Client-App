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
                cell.DetailTextLabel.Text = _parentModel.FormattedContractorStatusByRowNumber(rowNumber);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(ContractorJobStatusListViewController.CellIdentifier) as ProposedContractorsTableViewCell;

			cell.UpdateCell 
			(
				mainText: _parentModel.ContractorNameByRowNumber((int)indexPath.Item),
				billRate: _parentModel.FormattedBillRateByRowNumber((int)indexPath.Item),
				payRate: _parentModel.FormattedPayRateByRowNumber((int)indexPath.Item),
				grossMargin: _parentModel.FormattedGrossMarginByRowNumber((int)indexPath.Item),
				markup: _parentModel.FormattedMarkupByRowNumber((int)indexPath.Item)
			);
            //SetCellTextByRowNumber(cell, (int)indexPath.Item);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _parentModel.NumberOfContractors();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 56;
        }
    }
}