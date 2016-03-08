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
    public class ContractCreationSendingTableViewSource : UITableViewSource
    {
        private readonly ContractCreationSendingTableViewController _parentController;
        private readonly ContractCreationViewModel _contractModel;
        private float _specializationCellHeight = -1;

        public ContractCreationSendingTableViewSource(ContractCreationSendingTableViewController parentController,
            ContractCreationViewModel model)
        {
            _parentController = parentController;
            _contractModel = model;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            int row = (int) indexPath.Item;

            if (IsSendingConsultantContractCell(indexPath))
                return GetIsSendingConsultantContractCell(tableView, indexPath);
            else if (IsClientContactCell(indexPath))
                return GetClientContactCell(tableView, indexPath);
            else if (IsDirectReportCell(indexPath))
                return GetDirectReportCell(tableView, indexPath);
            else if (IsBillingContactCell(indexPath))
                return GetBillingContactCell(tableView, indexPath);
            else if (IsInvoiceRecipientsCell(indexPath))
                return GetInvoiceRecipientsCell(tableView, indexPath);
            else if (IsClientContractCell(indexPath))
                return GetClientContractCell(tableView, indexPath);

            EditableTextFieldCell cell =
                (EditableTextFieldCell) tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);

            return cell;
        }

        private int _isSendingConsultantContractCellRow
        {
            get { return 0; }
        }

        private bool IsSendingConsultantContractCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _isSendingConsultantContractCellRow;
        }

        private int _clientContactCellRow
        {
            get { return _isSendingConsultantContractCellRow + 1; }
        }

        private bool IsClientContactCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _clientContactCellRow;
        }

        private int _directReportCellRow
        {
            get { return _clientContactCellRow + 1; }
        }

        private bool IsDirectReportCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _directReportCellRow;
        }

        private int _billingContactCellRow
        {
            get { return _directReportCellRow + 1; }
        }

        private bool IsBillingContactCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _billingContactCellRow;
        }

        private int _invoiceRecipientsCellRow
        {
            get { return _billingContactCellRow + 1; }
        }

        private bool IsInvoiceRecipientsCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _invoiceRecipientsCellRow;
        }

        private int _clientContractCellRow
        {
            get { return _invoiceRecipientsCellRow + 1; }
        }

        private bool IsClientContractCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _clientContractCellRow;
        }


        private UITableViewCell GetIsSendingConsultantContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Send consultant e-contract to ________", new List<string> { "Yes", "No" }, 0);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.IsSendingConsultantContract = (newValue == "Yes"); };

            return cell;
        }

        private UITableViewCell GetClientContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Client Contact", _contractModel.ClientContactNameOptions, 0);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.ClientContactName = newValue; };

            return cell;
        }

        private UITableViewCell GetDirectReportCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Direct Report", _contractModel.DirectReportNameOptions, 0);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.DirectReportName = newValue; };

            return cell;
        }

        private UITableViewCell GetBillingContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Billing Contact", _contractModel.BillingContactNameOptions, 0);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.BillingContactName
                = newValue; };

            return cell;
        }

        private UITableViewCell GetInvoiceRecipientsCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell =
                (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Recipients", _contractModel.InvoiceRecipients);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.InvoiceRecipients = newValue;
            };

            return cell;
        }

        private UITableViewCell GetClientContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Send e-contract to: ____", new List<string> { "Yes", "No" }, 0);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.IsSendingContractToClientContact = (newValue == "Yes");
            };

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _clientContractCellRow + 1;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}