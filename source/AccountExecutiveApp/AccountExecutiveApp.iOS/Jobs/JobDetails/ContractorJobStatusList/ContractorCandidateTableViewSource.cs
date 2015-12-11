using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using SiSystems.SharedModels;
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
            if (_parentModel.Status == JobStatus.Shortlisted)
            {
                var cell = tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as RightDetailCell;

                cell.UpdateCell
                    (
                        mainText: _parentModel.ContractorNameByRowNumber((int)indexPath.Item),
                        rightDetailText: ""
                    );
                
                return cell;
            }
            else
            {
                var cell =
                    tableView.DequeueReusableCell(ContractorJobStatusListViewController.CellIdentifier) as
                        ProposedContractorsTableViewCell;

                cell.UpdateCell
                    (
                        mainText: _parentModel.ContractorNameByRowNumber((int) indexPath.Item),
						subtitleText: _parentModel.FormattedDateByRowNumber((int)indexPath.Item),
                        billRate: _parentModel.FormattedBillRateByRowNumber((int) indexPath.Item),
                        payRate: _parentModel.FormattedPayRateByRowNumber((int) indexPath.Item),
                        grossMargin: _parentModel.FormattedGrossMarginByRowNumber((int) indexPath.Item),
                        markup: _parentModel.FormattedMarkupByRowNumber((int) indexPath.Item)
                    );
                //SetCellTextByRowNumber(cell, (int)indexPath.Item);

                return cell;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _parentModel.NumberOfContractors();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (_parentModel.Status == JobStatus.Shortlisted)
                return 44;
            else
                return 85;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var vc = (ContractorDetailsTableViewController) _parentController.Storyboard.InstantiateViewController("ContractorDetailsTableViewController");
            vc.setContractorId(_parentModel.ContractorContactIdByRowNumber((int)indexPath.Item));
            _parentController.ShowViewController(vc, _parentController);
        }
    }
}