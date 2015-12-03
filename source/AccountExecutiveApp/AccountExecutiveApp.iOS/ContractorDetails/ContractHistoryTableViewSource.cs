using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ContractHistoryTableViewSource : UITableViewSource
    {
        private readonly ContractHistoryTableViewController _parentController;
        private ContractHistoryTableViewModel _tableModel;

        public ContractHistoryTableViewSource(ContractHistoryTableViewController parentController, IEnumerable<ConsultantContract> contracts )
        {
            _parentController = parentController;
            _tableModel = new ContractHistoryTableViewModel(contracts);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(ContractHistoryTableViewController.CellIdentifier) as SubtitleWithRightDetailCell;

            cell.UpdateCell(
                mainText: _tableModel.ContractTitleByRowNumber((int)indexPath.Item),
                subtitleText: _tableModel.CompanyNameByRowNumber((int)indexPath.Item),
                rightDetailText: _tableModel.ContractPeriodByRowNumber((int)indexPath.Item)
                );

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _tableModel.NumberOfContracts();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}