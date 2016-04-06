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
        private readonly ContractSendingSupportViewModel _supportModel;
        private float _specializationCellHeight = -1;

        //Table logic
        private bool _showClientContractCellReason = false;
        private bool _showClientContractOtherReason = false;

        public ContractCreationSendingTableViewSource(ContractCreationSendingTableViewController parentController,
            ContractCreationViewModel model, ContractSendingSupportViewModel supportModel)
        {
            _parentController = parentController;
            _contractModel = model;
            _supportModel = supportModel;
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
            else if (IsReasonCell(indexPath))
                return GetReasonCell(tableView, indexPath);
            else if (IsOtherReasonCell(indexPath))
                return GetOtherReasonCell(tableView, indexPath);

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

        private int _reasonCellRow
        {
            get { return _clientContractCellRow + 1; }
        }

        private bool IsReasonCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _reasonCellRow;
        }

        private int _otherReasonCellRow
        {
            get { return _reasonCellRow + 1; }
        }

        private bool IsOtherReasonCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _otherReasonCellRow;
        }

        private UITableViewCell GetIsSendingConsultantContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell( string.Format("Send consultant e-contract to {0}:", _contractModel.ConsultantName), _contractModel.BooleanOptions, _contractModel.IsSendingConsultantContractSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.IsSendingConsultantContract = (newValue == "Yes"); };

            return cell;
        }

        private UITableViewCell GetClientContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Client Contact", _contractModel.ClientContactNameOptions, _contractModel.ClientContactNameSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetClientContact(_contractModel.GetClientContactWithName(newValue)); };

            return cell;
        }

        private UITableViewCell GetDirectReportCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Direct Report", _contractModel.DirectReportNameOptions, _contractModel.DirectReportNameSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetDirectReport( _contractModel.GetDirectReportWithName(newValue) ); };

            return cell;
        }

        private UITableViewCell GetBillingContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Billing Contact", _contractModel.BillingContactNameOptions, _contractModel.BillingContactNameSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetBillingContact(_contractModel.GetBillingContactWithName(newValue)); };

            return cell;
        }

        private UITableViewCell GetInvoiceRecipientsCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                (MultiSelectDescriptionCell)tableView.DequeueReusableCell(MultiSelectDescriptionCell.CellIdentifier, indexPath);

            cell.UpdateCell("Invoice Recipients", _contractModel.Contract.InvoiceRecipients.Select(c => c.FullName).ToList() );

            return cell;
        }

        private UITableViewCell GetClientContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDoublePickerCell cell = (EditableDoublePickerCell)tableView.DequeueReusableCell(EditableDoublePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell(string.Format("Send e-contract to"), _contractModel.ClientContractContactNameOptions, _contractModel.ClientContractContactNameSelectedIndex, _contractModel.BooleanOptions, _contractModel.IsSendingContractToClientContactSelectedIndex);

            cell.OnMidValueChanged += delegate(string newValue)
            {
                _contractModel.SetClientContractContact(_contractModel.GetClientContractContactWithName(newValue));
            };
            
            cell.OnRightValueChanged += delegate(string newValue)
            {
                bool isSending = (newValue == "Yes");
                _contractModel.IsSendingContractToClientContact = isSending;

                EvaluateDynamicCells( tableView );
            };

            return cell;
        }

        private UITableViewCell GetReasonCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Reason:", _contractModel.ReasonForNotSendingContractOptions, _contractModel.ReasonForNotSendingContractSelectedIndex);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.ReasonForNotSendingContract = newValue;

                EvaluateDynamicCells( tableView );
            };

            return cell;
        }

        private UITableViewCell GetOtherReasonCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableFullTextFieldCell cell = (EditableFullTextFieldCell)tableView.DequeueReusableCell(EditableFullTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell(_contractModel.SummaryReasonForNotSendingContract);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.SummaryReasonForNotSendingContract = newValue;
            };

            return cell;
        }

        private void EvaluateDynamicCells(UITableView tableView)
        {
            bool isSending = _contractModel.IsSendingContractToClientContact;

            _showClientContractCellReason = !isSending;

            if (isSending == true)
                _showClientContractOtherReason = false;

            if (isSending == false)
            {
                if (_contractModel.ReasonForNotSendingContract == "Other")
                    _showClientContractOtherReason = true;
                else
                    _showClientContractOtherReason = false;
            }

            if (!_showClientContractOtherReason)
                _contractModel.SummaryReasonForNotSendingContract = string.Empty;//reset summary reason if there is no reason anymore

            tableView.ReloadData();
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if ( _showClientContractCellReason == false )
                return _clientContractCellRow + 1;
            else if( _showClientContractOtherReason == false )
                return _clientContractCellRow + 2;
            else if ( _showClientContractOtherReason == true )
                return _clientContractCellRow + 3;

            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsInvoiceRecipientsCell(indexPath))
            {
                MultiSelectTableViewController vc = new MultiSelectTableViewController();
                vc.SetData( _contractModel.InvoiceRecipientOptions.ToList(), _contractModel.Contract.InvoiceRecipients.ToList() );//set list of options, and then currently selected list
                vc.OnSelectionChanged = delegate(List<UserContact> selected)
                {
                    _contractModel.Contract.InvoiceRecipients = selected.AsEnumerable();
                    tableView.ReloadData();
                };
                _parentController.ShowViewController(vc, _parentController);
            }
        }
    }
}