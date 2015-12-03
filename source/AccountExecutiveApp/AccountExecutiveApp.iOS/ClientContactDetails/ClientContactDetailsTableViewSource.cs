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
    public class ClientContactDetailsTableViewSource : UITableViewSource
    {
        private readonly ClientContactDetailsTableViewController _parentController;
        private readonly ClientContactDetailsTableViewModel _parentModel;
        private ClientContactDetailsTableViewModel _tableModel;

        public ClientContactDetailsTableViewSource(ClientContactDetailsTableViewController parentController, UserContact ClientContact)
        {
            _parentController = parentController;
            _tableModel = new ClientContactDetailsTableViewModel(ClientContact);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsCallOrTextCell(indexPath))
                return GetCallOrTextContactCell(tableView, indexPath);
            else if (IsEmailCell(indexPath))
                return GetEmailContactCell(tableView, indexPath);

            return null;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1 + 2;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            /*
            if (IsContractsCell(indexPath))
            {
                ContractHistoryTableViewController vc =
                    (ContractHistoryTableViewController)
                        _parentController.Storyboard.InstantiateViewController("ContractHistoryTableViewController");

                vc.setContracts(_tableModel.Contracts);

                _parentController.ShowViewController(vc, _parentController);
            }*/
        }

        private UITableViewCell GetEmailContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(ClientContactDetailsTableViewController.CellIdentifier) as
                    ContractorContactInfoCell;

            cell.ParentViewController = _parentController;

            cell.UpdateCell
                (
                    mainContactText:
                        _tableModel.FormattedEmailByRowNumber((int)indexPath.Item -
                                                              _tableModel.NumberOfPhoneNumbers()),
                    contactTypeText: "Home",
                    canPhone: false,
                    canText: false,
                    canEmail: true
                );

            return cell;
        }

        private UITableViewCell GetCallOrTextContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(ClientContactDetailsTableViewController.CellIdentifier) as
                    ContractorContactInfoCell;

            cell.ParentViewController = _parentController;

            cell.UpdateCell
                (
                    mainContactText: _tableModel.FormattedPhoneNumberByRowNumber((int)indexPath.Item),
                    contactTypeText: "Mobile",
                    canPhone: true,
                    canText: true,
                    canEmail: false
                );

            return cell;
        }

        //private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
        //private bool IsSpecializationCell(NSIndexPath indexPath) { return (int)indexPath.Item == _specializationCellRow; }

        private int _firstPhoneNumberCellIndex { get { return 0; } }
        private int _numberOfPhoneNumberCells { get { return _tableModel.NumberOfPhoneNumbers(); } }
        private bool IsCallOrTextCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _firstPhoneNumberCellIndex && (int)indexPath.Item < _firstPhoneNumberCellIndex + _numberOfPhoneNumberCells)
                return true;
            return false;
        }

        private int _firstEmailCellIndex { get { return _numberOfPhoneNumberCells; } }
        private int _numberOfEmailCells { get { return _tableModel.NumberOfEmails(); } }
        private bool IsEmailCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _firstEmailCellIndex && (int)indexPath.Item < _firstEmailCellIndex + _numberOfEmailCells)
                return true;
            return false;
        }
    }
}