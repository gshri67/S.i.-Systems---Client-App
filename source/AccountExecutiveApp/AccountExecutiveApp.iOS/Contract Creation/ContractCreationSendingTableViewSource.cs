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
            else if (IsEndDateCell(indexPath))
                return GetEndDateCell(tableView, indexPath);
            else if (IsTimeFactorCell(indexPath))
                return GetTimeFactorCell(tableView, indexPath);
            else if (IsDaysCancellationCell(indexPath))
                return GetDaysCancellationCell(tableView, indexPath);
            else if (IsLimitationExpenseCell(indexPath))
                return GetLimitationExpenseCell(tableView, indexPath);
            else if (IsLimitationOfContractCell(indexPath))
                return GetLimitationOfContractCell(tableView, indexPath);
            else if (IsPaymentPlanCell(indexPath))
                return GetPaymentPlanCell(tableView, indexPath);
            else if (IsAccountExecutiveCell(indexPath))
                return GetAccountExecutiveCell(tableView, indexPath);
            else if (IsGMAssignedCell(indexPath))
                return GetGMAssignedCell(tableView, indexPath);
            else if (IsComissionAssignedCell(indexPath))
                return GetCommisionAssignedCell(tableView, indexPath);
            else if (IsInvoiceFrequencyCell(indexPath))
                return GetInvoiceFrequencyCell(tableView, indexPath);
            else if (IsInvoiceFormatCell(indexPath))
                return GetInvoiceFormatCell(tableView, indexPath);
            else if (IsProjectCodeCell(indexPath))
                return GetProjectCodesCell(tableView, indexPath);
            else if (IsQuickPayCell(indexPath))
                return GetQuickPayCell(tableView, indexPath);

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

        private int _endDateCellRow
        {
            get { return _startDateCellRow + 1; }
        }

        private bool IsEndDateCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _endDateCellRow;
        }

        private int _timeFactorCellRow
        {
            get { return _endDateCellRow + 1; }
        }

        private bool IsTimeFactorCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _timeFactorCellRow;
        }

        private int _daysCancellationCellRow
        {
            get { return _timeFactorCellRow + 1; }
        }

        private bool IsDaysCancellationCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _daysCancellationCellRow;
        }

        private int _limitationExpenseCellRow
        {
            get { return _daysCancellationCellRow + 1; }
        }

        private bool IsLimitationExpenseCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _limitationExpenseCellRow;
        }

        private int _limitationOfContractCellRow
        {
            get { return _limitationExpenseCellRow + 1; }
        }

        private bool IsLimitationOfContractCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _limitationOfContractCellRow;
        }

        private int _paymentPlanCellRow
        {
            get { return _limitationOfContractCellRow + 1; }
        }

        private bool IsPaymentPlanCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _paymentPlanCellRow;
        }

        private int _accountExecutiveCellRow
        {
            get { return _paymentPlanCellRow + 1; }
        }

        private bool IsAccountExecutiveCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _accountExecutiveCellRow;
        }

        private int _GMAssignedCellRow
        {
            get { return _accountExecutiveCellRow + 1; }
        }

        private bool IsGMAssignedCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _GMAssignedCellRow;
        }

        private int _comissionAssignedCellRow
        {
            get { return _GMAssignedCellRow + 1; }
        }

        private bool IsComissionAssignedCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _comissionAssignedCellRow;
        }

        private int _invoiceFrequencyCellRow
        {
            get { return _comissionAssignedCellRow + 1; }
        }

        private bool IsInvoiceFrequencyCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _invoiceFrequencyCellRow;
        }

        private int _invoiceFormatCellRow
        {
            get { return _invoiceFrequencyCellRow + 1; }
        }

        private bool IsInvoiceFormatCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _invoiceFormatCellRow;
        }

        private int _projectCodeCellRow
        {
            get { return _invoiceFormatCellRow + 1; }
        }

        private bool IsProjectCodeCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _projectCodeCellRow;
        }

        private int _quickPayCellRow
        {
            get { return _projectCodeCellRow + 1; }
        }

        private bool IsQuickPayCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _quickPayCellRow;
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
            cell.UpdateCell("Client Contact", new List<string> { "Yes", "No" }, 0);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.ClientContactName = newValue; };

            return cell;
        }

        private UITableViewCell GetEndDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("End Date", _contractModel.FormattedEndDate);
            cell.OnValueChanged += delegate(DateTime newValue) { _contractModel.EndDate = newValue; };

            return cell;
        }

        private UITableViewCell GetTimeFactorCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Time Factor", _contractModel.TimeFactorOptions, _contractModel.TimeFactorSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.TimeFactor = newValue; };

            return cell;
        }

        private UITableViewCell GetDaysCancellationCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Days Cancellation", _contractModel.DaysCancellationOptions,
                _contractModel.DaysCancellationSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.DaysCancellation = int.Parse(newValue); };

            return cell;
        }

        private UITableViewCell GetLimitationExpenseCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation Expense", _contractModel.LimitationExpenseOptions,
                _contractModel.LimitationExpenseSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.LimitationExpense = newValue; };

            return cell;
        }

        private UITableViewCell GetLimitationOfContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation of Contract", _contractModel.LimitationOfContractOptions,
                _contractModel.LimitationOfContractSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.LimitationOfContract = newValue; };

            return cell;
        }

        private UITableViewCell GetPaymentPlanCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Payment Plan", _contractModel.PaymentPlanOptions, _contractModel.PaymentPlanSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.PaymentPlan = newValue; };

            return cell;
        }

        private UITableViewCell GetAccountExecutiveCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            List<UserContact> accountExecutives =
                new List<UserContact>(new UserContact[] {new UserContact(), new UserContact(), new UserContact()});
            cell.UpdateCell("Account Executive", _contractModel.AccountExecutiveOptionDescriptions,
                _contractModel.AccountExecutiveIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetAccountExecutiveWithName(newValue); };

            return cell;
        }

        private UITableViewCell GetGMAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("GM Assigned", _contractModel.GMAssignedOptionDescriptions, _contractModel.GMAssignedIndex);
            return cell;
        }

        private UITableViewCell GetCommisionAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Comission Assigned", _contractModel.ComissionAssignedOptionDescriptions,
                _contractModel.ComissionAssignedIndex);
            return cell;
        }

        private UITableViewCell GetInvoiceFrequencyCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Frequency", _contractModel.InvoiceFrequencyOptions,
                _contractModel.InvoiceFrequencySelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFrequency = newValue; };

            return cell;
        }

        private UITableViewCell GetInvoiceFormatCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Format", _contractModel.InvoiceFormatOptions,
                _contractModel.InvoiceFormatSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFormat = newValue; };

            return cell;
        }

        private UITableViewCell GetProjectCodesCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell) tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Project/PO codes required", _contractModel.UsingProjectCode);
            cell.OnValueChanged += delegate(bool newValue) { _contractModel.UsingProjectCode = newValue; };

            return cell;
        }

        private UITableViewCell GetQuickPayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell) tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Quick Pay", _contractModel.UsingQuickPay);
            cell.OnValueChanged += delegate(bool newValue) { _contractModel.UsingQuickPay = newValue; };

            return cell;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            ContractCreationDetailsHeaderView createContractHeader =
                new ContractCreationDetailsHeaderView(
                    "Please use the Matchguide System if you want to use Third Party Billing to create a contract");

            tableView.TableHeaderView = createContractHeader;
            return createContractHeader;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 100.0f;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _quickPayCellRow + 1;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}