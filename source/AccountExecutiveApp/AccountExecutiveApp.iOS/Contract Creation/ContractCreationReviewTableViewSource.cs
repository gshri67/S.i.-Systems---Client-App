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
    public class ContractCreationReviewTableViewSource : UITableViewSource
    {
        private readonly ContractCreationReviewTableViewController _parentController;
        private readonly ContractCreationViewModel _contractModel;

        public ContractCreationReviewTableViewSource(ContractCreationReviewTableViewController parentController,
            ContractCreationViewModel model)
        {
            _parentController = parentController;
            _contractModel = model;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            int row = (int)indexPath.Item;

            if (indexPath.Section == 0)
            {
                if (IsJobTitleCell(indexPath))
                    return GetJobTitleCell(tableView, indexPath);
                else if (IsStartDateCell(indexPath))
                    return GetStartDateCell(tableView, indexPath);
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
            }
            else if (indexPath.Section < _contractModel.NumRates + 1)
            {
                if (IsRateTypeCell(indexPath))
                    return GetRateTypeCell(tableView, indexPath);
                else if (IsRateDescriptionCell(indexPath))
                    return GetRateDescriptionCell(tableView, indexPath);
                else if (IsBillRateCell(indexPath))
                    return GetBillRateCell(tableView, indexPath);
                else if (IsIsPrimaryRateCell(indexPath))
                    return GetIsPrimaryRateCell(tableView, indexPath);
            }

            EditableTextFieldCell cell =
                (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);

            return cell;
        }

        private int _jobTitleCellRow
        {
            get { return 0; }
        }

        private bool IsJobTitleCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _jobTitleCellRow;
        }

        private int _startDateCellRow
        {
            get { return _jobTitleCellRow + 1; }
        }

        private bool IsStartDateCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _startDateCellRow;
        }

        private int _endDateCellRow
        {
            get { return _startDateCellRow + 1; }
        }

        private bool IsEndDateCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _endDateCellRow;
        }

        private int _timeFactorCellRow
        {
            get { return _endDateCellRow + 1; }
        }

        private bool IsTimeFactorCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _timeFactorCellRow;
        }

        private int _daysCancellationCellRow
        {
            get { return _timeFactorCellRow + 1; }
        }

        private bool IsDaysCancellationCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _daysCancellationCellRow;
        }

        private int _limitationExpenseCellRow
        {
            get { return _daysCancellationCellRow + 1; }
        }

        private bool IsLimitationExpenseCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _limitationExpenseCellRow;
        }

        private int _limitationOfContractCellRow
        {
            get { return _limitationExpenseCellRow + 1; }
        }

        private bool IsLimitationOfContractCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _limitationOfContractCellRow;
        }

        private int _paymentPlanCellRow
        {
            get { return _limitationOfContractCellRow + 1; }
        }

        private bool IsPaymentPlanCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _paymentPlanCellRow;
        }

        private int _accountExecutiveCellRow
        {
            get { return _paymentPlanCellRow + 1; }
        }

        private bool IsAccountExecutiveCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _accountExecutiveCellRow;
        }

        private int _GMAssignedCellRow
        {
            get { return _accountExecutiveCellRow + 1; }
        }

        private bool IsGMAssignedCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _GMAssignedCellRow;
        }

        private int _comissionAssignedCellRow
        {
            get { return _GMAssignedCellRow + 1; }
        }

        private bool IsComissionAssignedCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _comissionAssignedCellRow;
        }

        private int _invoiceFrequencyCellRow
        {
            get { return _comissionAssignedCellRow + 1; }
        }

        private bool IsInvoiceFrequencyCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _invoiceFrequencyCellRow;
        }

        private int _invoiceFormatCellRow
        {
            get { return _invoiceFrequencyCellRow + 1; }
        }

        private bool IsInvoiceFormatCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _invoiceFormatCellRow;
        }

        private int _projectCodeCellRow
        {
            get { return _invoiceFormatCellRow + 1; }
        }

        private bool IsProjectCodeCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _projectCodeCellRow;
        }

        private int _quickPayCellRow
        {
            get { return _projectCodeCellRow + 1; }
        }

        private bool IsQuickPayCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _quickPayCellRow;
        }

        private int _numInitialPageCells
        {
            get { return _quickPayCellRow + 1; }
        }

//Contract Rates Page Indices

        private int _localRateTypeCellRow { get { return 0; } }
        private bool IsRateTypeCell(NSIndexPath indexPath) { return (int)indexPath.Item == _localRateTypeCellRow; }

        private int _localRateDescriptionCellRow { get { return _localRateTypeCellRow + 1; } }
        private bool IsRateDescriptionCell(NSIndexPath indexPath) { return (int)indexPath.Item == _localRateDescriptionCellRow; }

        private int _localBillRateCellRow { get { return _localRateDescriptionCellRow + 1; } }
        private bool IsBillRateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _localBillRateCellRow; }

        private int _localIsPrimaryRateCellRow { get { return _localBillRateCellRow + 1; } }
        private bool IsIsPrimaryRateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _localIsPrimaryRateCellRow; }

        private int _numCellsPerRateSection { get { return _localIsPrimaryRateCellRow + 1; } }


//Get Cells for Initial Page
        private UITableViewCell GetJobTitleCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell =
                (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Job Title", _contractModel.JobTitle);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.JobTitle = newValue; };

            return cell;
        }

        private UITableViewCell GetStartDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Start Date", _contractModel.FormattedStartDate);
            cell.OnValueChanged += delegate(DateTime newValue) { _contractModel.StartDate = newValue; };

            return cell;
        }

        private UITableViewCell GetEndDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("End Date", _contractModel.FormattedEndDate);
            cell.OnValueChanged += delegate(DateTime newValue) { _contractModel.EndDate = newValue; };

            return cell;
        }

        private UITableViewCell GetTimeFactorCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Time Factor", _contractModel.TimeFactorOptions, _contractModel.TimeFactorSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.TimeFactor = newValue; };

            return cell;
        }

        private UITableViewCell GetDaysCancellationCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Days Cancellation", _contractModel.DaysCancellationOptions,
                _contractModel.DaysCancellationSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.DaysCancellation = int.Parse(newValue); };

            return cell;
        }

        private UITableViewCell GetLimitationExpenseCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation Expense", _contractModel.LimitationExpenseOptions,
                _contractModel.LimitationExpenseSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.LimitationExpense = newValue; };

            return cell;
        }

        private UITableViewCell GetLimitationOfContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation of Contract", _contractModel.LimitationOfContractOptions,
                _contractModel.LimitationOfContractSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.LimitationOfContract = newValue; };

            return cell;
        }

        private UITableViewCell GetPaymentPlanCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Payment Plan", _contractModel.PaymentPlanOptions, _contractModel.PaymentPlanSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.PaymentPlan = newValue; };

            return cell;
        }

        private UITableViewCell GetAccountExecutiveCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            List<UserContact> accountExecutives =
                new List<UserContact>(new UserContact[] { new UserContact(), new UserContact(), new UserContact() });
            cell.UpdateCell("Account Executive", _contractModel.AccountExecutiveOptionDescriptions,
                _contractModel.AccountExecutiveIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetAccountExecutiveWithName(newValue); };

            return cell;
        }

        private UITableViewCell GetGMAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("GM Assigned", _contractModel.GMAssignedOptionDescriptions, _contractModel.GMAssignedIndex);
            return cell;
        }

        private UITableViewCell GetCommisionAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Comission Assigned", _contractModel.ComissionAssignedOptionDescriptions,
                _contractModel.ComissionAssignedIndex);
            return cell;
        }

        private UITableViewCell GetInvoiceFrequencyCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Frequency", _contractModel.InvoiceFrequencyOptions,
                _contractModel.InvoiceFrequencySelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFrequency = newValue; };

            return cell;
        }

        private UITableViewCell GetInvoiceFormatCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Format", _contractModel.InvoiceFormatOptions,
                _contractModel.InvoiceFormatSelectedIndex);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFormat = newValue; };

            return cell;
        }

        private UITableViewCell GetProjectCodesCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Project/PO codes required", _contractModel.BooleanOptions, _contractModel.BooleanOptionIndex(_contractModel.UsingProjectCode));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.UsingProjectCode = (newValue == "Yes"); };

            return cell;
        }

        private UITableViewCell GetQuickPayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Quick Pay", _contractModel.BooleanOptions, _contractModel.BooleanOptionIndex(_contractModel.UsingQuickPay));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.UsingQuickPay = (newValue == "Yes"); };

            return cell;
        }


//Get Cells for Contract Rates
        private UITableViewCell GetRateTypeCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.UpdateCell("Rate Type", _contractModel.RateTypeOptions, _contractModel.RateTypeSelectedIndexAtIndex(ContractRatesSectionLocalIndex(indexPath)));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateTypeAtIndex(newValue, ContractRatesSectionLocalIndex(indexPath)); };

            return cell;
        }

        private UITableViewCell GetRateDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Rate Description", _contractModel.RateDescriptionOptions, _contractModel.RateDescriptionSelectedIndexAtIndex(ContractRatesSectionLocalIndex(indexPath)));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateDescriptionAtIndex(newValue, ContractRatesSectionLocalIndex(indexPath)); };

            return cell;
        }

        private UITableViewCell GetBillRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Bill Rate", _contractModel.BillRateAtIndex(ContractRatesSectionLocalIndex(indexPath)));
            cell.OnValueChanged += delegate(float newValue)
            {
                _contractModel.SetBillRateAtIndex(newValue.ToString(), ContractRatesSectionLocalIndex(indexPath));
            };

            return cell;
        }

        private UITableViewCell GetIsPrimaryRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Primary Rate", _contractModel.IsPrimaryRateAtIndex(ContractRatesSectionLocalIndex(indexPath)));
            cell.OnValueChanged += delegate(bool newValue) { _contractModel.SetPrimaryRateForIndex(ContractRatesSectionLocalIndex(indexPath)); };

            return cell;
        }
       

//indexing methods
        private int ContractRatesSectionLocalIndex(NSIndexPath indexPath)
        {
            return (int)indexPath.Section - 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if( (int)section == 0 )
                return _numInitialPageCells;
            else if ((int) section < _contractModel.NumRates + 1)
                return _numCellsPerRateSection;
            else return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1 + _contractModel.NumRates;
        }
    }
}
