﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ContractCreationDetailsTableViewSource : UITableViewSource
    {
        private readonly ContractCreationDetailsTableViewController _parentController;
        private readonly ContractCreationViewModel _contractModel;
        private readonly ContractBodySupportViewModel _supportModel;
        private float _specializationCellHeight = -1;

        public ContractCreationDetailsTableViewSource(ContractCreationDetailsTableViewController parentController,
            ContractCreationViewModel model, ContractBodySupportViewModel supportModel )
        {
            _parentController = parentController;
            _contractModel = model;
            _supportModel = supportModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            int row = (int) indexPath.Item;

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
            else if (IsBranchCell(indexPath))
                return GetBranchCell(tableView, indexPath);
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

        private int _jobTitleCellRow
        {
            get { return 0; }
        }

        private bool IsJobTitleCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _jobTitleCellRow;
        }

        private int _startDateCellRow
        {
            get { return _jobTitleCellRow + 1; }
        }

        private bool IsStartDateCell(NSIndexPath indexPath)
        {
            return (int) indexPath.Item == _startDateCellRow;
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

        private int _branchCellRow
        {
            get { return _paymentPlanCellRow + 1; }
        }

        private bool IsBranchCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _branchCellRow;
        }

        private int _accountExecutiveCellRow
        {
            get { return _branchCellRow + 1; }
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


        private UITableViewCell GetJobTitleCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell =
                (EditableTextFieldCell) tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Job Title", _contractModel.JobTitle);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.JobTitle = newValue; };

            return cell;
        }

        private UITableViewCell GetStartDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell) tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Start Date", _contractModel.FormattedStartDate);
            cell.OnValueChanged += delegate(DateTime newValue) { _contractModel.StartDate = newValue; };

            return cell;
        }

        private UITableViewCell GetEndDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell) tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("End Date", _contractModel.FormattedEndDate);
            cell.OnValueChanged += delegate(DateTime newValue) { _contractModel.EndDate = newValue; };

            return cell;
        }

        private UITableViewCell GetTimeFactorCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.TimeFactor = newValue; };
            cell.UpdateCell("Time Factor", _supportModel.TimeFactorOptions, _contractModel.Contract.TimeFactor);

            return cell;
        }

        private UITableViewCell GetDaysCancellationCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Days Cancellation", _supportModel.DaysCancellationOptions,
                _contractModel.DaysCancellation);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.DaysCancellation = newValue;
            };

            return cell;
        }

        private UITableViewCell GetLimitationExpenseCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell = (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UsingDollarSign = true;
            cell.OnValueChanged += delegate(float newValue) { };
            cell.OnValueFinalized += delegate(float newValue) { _contractModel.LimitationExpense = newValue.ToString(); };
            cell.UpdateCell("Limitation Expense", _contractModel.LimitationExpense);

            return cell;
        }

        private UITableViewCell GetLimitationOfContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerWithNumberFieldValueCell cell = (EditablePickerWithNumberFieldValueCell)tableView.DequeueReusableCell(EditablePickerWithNumberFieldValueCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation of Contract", _supportModel.LimitationOfContractTypeOptions,
                _contractModel.LimitationOfContractType,
               _contractModel.LimitationOfContractValue.ToString());

            cell.OnMidValueChanged += delegate(string newValue)
            {
                _contractModel.LimitationOfContractType = newValue;
            };

            cell.OnRightValueChanged += delegate(float newValue)
            {
                _contractModel.LimitationOfContractValue = (int)newValue;
            };

            return cell;
        }

        private UITableViewCell GetPaymentPlanCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.OnValueChanged += delegate(string newValue) { _contractModel.PaymentPlan = newValue; };
            cell.UpdateCell("Payment Plan", _supportModel.PaymentPlanOptions, _contractModel.PaymentPlan);

            return cell;
        }

        private UITableViewCell GetBranchCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.Branch = newValue;

                Task task = _supportModel.UpdateColleaguesWithBranch( int.Parse(newValue));
                task.ContinueWith(_ => InvokeOnMainThread(tableView.ReloadData), TaskContinuationOptions.OnlyOnRanToCompletion);
            };
            cell.UpdateCell("Branch", _supportModel.BranchOptions, _contractModel.Branch);

            return cell;
        }

        private UITableViewCell GetAccountExecutiveCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.AccountExecutive = _supportModel.GetAccountExecutiveWithName(newValue);
            };
            cell.UpdateCell("Account Executive", _supportModel.AccountExecutiveOptionDescriptions,
                _contractModel.AccountExecutive.FullName);

            return cell;
        }

        private UITableViewCell GetGMAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.GMAssigned = _supportModel.GetGMAssignedWithName(newValue);
            };
            cell.UpdateCell("GM Assigned", _supportModel.GMAssignedOptionDescriptions, _contractModel.GMAssigned.FullName);
            return cell;
        }

        private UITableViewCell GetCommisionAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.ComissionAssigned = _supportModel.GetComissionAssignedWithName(newValue);
            };
            cell.UpdateCell("Comission Assigned", _supportModel.ComissionAssignedOptionDescriptions,
                _contractModel.ComissionAssigned.FullName);
            return cell;
        }

        private UITableViewCell GetInvoiceFrequencyCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFrequency = newValue; };
            cell.UpdateCell("Invoice Frequency", _supportModel.InvoiceFrequencyOptions,
                _contractModel.InvoiceFrequency);

            return cell;
        }

        private UITableViewCell GetInvoiceFormatCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell) tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.InvoiceFormat = newValue; };
            cell.UpdateCell("Invoice Format", _supportModel.InvoiceFormatOptions,
                _contractModel.InvoiceFormat);
            

            return cell;
        }

        private UITableViewCell GetProjectCodesCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.UsingProjectCode = (newValue == "Yes"); };
            cell.UpdateCell("Project/PO codes required", _contractModel.UsingProjectCode);

            return cell;
        }

        private UITableViewCell GetQuickPayCell(UITableView tableView, NSIndexPath indexPath)
        {            
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.UsingQuickPay = (newValue == "Yes"); };
            cell.UpdateCell("Quick Pay", _contractModel.UsingQuickPay);
            
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsAccountExecutiveCell(indexPath))
            {
                SingleSelectTableViewController vc = new SingleSelectTableViewController();
                vc.SetData(_supportModel.AccountExecutiveOptions.ToList(), _contractModel.AccountExecutive.Id);
                vc.OnSelectionChanged = delegate(InternalEmployee selected)
                {
                    _contractModel.AccountExecutive = _supportModel.GetAccountExecutiveWithName(selected.FullName);
                    tableView.ReloadData();
                };
                _parentController.ShowViewController(vc, _parentController);
            }
            else if (IsComissionAssignedCell(indexPath))
            {
                SingleSelectTableViewController vc = new SingleSelectTableViewController();
                vc.SetData(_supportModel.ComissionAssignedOptions.ToList(), _contractModel.ComissionAssigned.Id);
                vc.OnSelectionChanged = delegate(InternalEmployee selected)
                {
                    _contractModel.ComissionAssigned = _supportModel.GetComissionAssignedWithName(selected.FullName);
                    tableView.ReloadData();
                };
                _parentController.ShowViewController(vc, _parentController);
            }
            else if (IsGMAssignedCell(indexPath))
            {
                SingleSelectTableViewController vc = new SingleSelectTableViewController();
                vc.SetData(_supportModel.GMAssignedOptions.ToList(), _contractModel.GMAssigned.Id);
                vc.OnSelectionChanged = delegate(InternalEmployee selected)
                {
                    _contractModel.GMAssigned = _supportModel.GetGMAssignedWithName(selected.FullName);
                    tableView.ReloadData();
                };
                _parentController.ShowViewController(vc, _parentController);
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            ContractCreationDetailsHeaderView createContractHeader =
                new ContractCreationDetailsHeaderView(
                    "Please use Matchguide if you want to use Third Party Billing to create a contract");

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