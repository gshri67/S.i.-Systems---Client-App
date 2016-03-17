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
            else if (indexPath.Section < _contractModel.NumRates + 2)
            {
                if (IsClientContactCell(indexPath))
                    return GetClientContactCell(tableView, indexPath);
                else if (IsDirectReportCell(indexPath))
                    return GetDirectReportCell(tableView, indexPath);
                else if (IsBillingContactCell(indexPath))
                    return GetBillingContactCell(tableView, indexPath);
                else if (IsInvoiceRecipientsCell(indexPath))
                    return GetInvoiceRecipientsCell(tableView, indexPath);
            }
            else if (indexPath.Section < _contractModel.NumRates + 3)//Recipients
                return null;
            else if (indexPath.Section < _contractModel.NumRates + 4)//Associated POs
                return null;
            else if (indexPath.Section < _contractModel.NumRates + 5) //Email
            {
                if (IsSendingConsultantContractCell(indexPath))
                    return GetIsSendingConsultantContractCell(tableView, indexPath);
                else if (IsEmailCell(indexPath))
                    return GetEmailCell(tableView, indexPath);
                else if (IsClientContractCell(indexPath))
                    return GetClientContractCell(tableView, indexPath);
                else if (IsReasonCell(indexPath))
                    return GetReasonCell(tableView, indexPath);
                else if (IsOtherReasonCell(indexPath))
                    return GetOtherReasonCell(tableView, indexPath);
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

//Sending Page Indices

        private int _clientContactCellRow
        {
            get { return 0; }
        }

        private bool IsClientContactCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _clientContactCellRow;
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

        private int _numSendingPageCells {
            get
            {
     
                return _invoiceRecipientsCellRow + 1;

                return 0;
            } 
        }

//Email Section Indices


        private bool _showEmailCell
        {
            get
            {
                if (_contractModel.IsSendingConsultantContract == true)
                    return true;
                return false;
            }
        }

        private bool _showClientContractCellReason
        {
            get
            {
                if (_contractModel.IsSendingContractToClientContact == false)
                    return true;
                return false;
            }
        }
        private bool _showClientContractOtherReason
        {
            get
            {
                if (_contractModel.ReasonForNotSendingContract == "Other")
                    return true;
                return false;
            }
        }


        private int _isSendingConsultantContractCellRow
        {
            get { return 0; }
        }

        private bool IsSendingConsultantContractCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _isSendingConsultantContractCellRow;
        }

        private int _emailCellRow
        {
            get { return _isSendingConsultantContractCellRow+1; }
        }

        private bool IsEmailCell(NSIndexPath indexPath)
        {
            return (int)indexPath.Item == _emailCellRow;
        }
         
        private int _clientContractCellRow
        {
            get
            {
                if (_showEmailCell)
                    return _emailCellRow + 1;
                else
                    return _emailCellRow;
            }
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
          
         
        private int _numEmailPageCells {
            get
            {
                if ( _showClientContractCellReason == false )
                    return _clientContractCellRow + 1;
                else if( _showClientContractOtherReason == false )
                    return _clientContractCellRow + 2;
                else if ( _showClientContractOtherReason == true )
                    return _clientContractCellRow + 3;

                return 0;
            } 
        }


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
      
//Get Sending Page cells
        private UITableViewCell GetClientContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Client Contact", _contractModel.ClientContactNameOptions, _contractModel.ClientContactNameSelectedIndex);
     
            return cell;
        }

        private UITableViewCell GetDirectReportCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Direct Report", _contractModel.DirectReportNameOptions, _contractModel.DirectReportNameSelectedIndex);

            return cell;
        }

        private UITableViewCell GetBillingContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Billing Contact", _contractModel.BillingContactNameOptions, _contractModel.BillingContactNameSelectedIndex);


            return cell;
        }

        private UITableViewCell GetInvoiceRecipientsCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell =
                (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Recipients", _contractModel.InvoiceRecipients);


            return cell;
        }

//Get Email Section Cells
        private UITableViewCell GetIsSendingConsultantContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell(string.Format("Send consultant e-contract to {0}:", _contractModel.ConsultantName), _contractModel.BooleanOptions, _contractModel.IsSendingConsultantContractSelectedIndex);

            return cell;
        }

        private UITableViewCell GetEmailCell(UITableView tableView, NSIndexPath indexPath)
        {
            EmailCell cell = (EmailCell)tableView.DequeueReusableCell(EmailCell.CellIdentifier, indexPath);
            cell.UpdateCell("Agreement Between .. ..", "Dear Jean-Claude, You are invited by S.i. Systems...");

            return cell;
        }

        private UITableViewCell GetClientContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDoublePickerCell cell = (EditableDoublePickerCell)tableView.DequeueReusableCell(EditableDoublePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell(string.Format("Send e-contract to"), _contractModel.ClientContractContactNameOptions, _contractModel.ClientContractContactNameSelectedIndex, _contractModel.BooleanOptions, _contractModel.IsSendingContractToClientContactSelectedIndex);


            return cell;
        }

        private UITableViewCell GetReasonCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Reason:", _contractModel.ReasonForNotSendingContractOptions, _contractModel.ReasonForNotSendingContractSelectedIndex);

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



//indexing methods
        private int ContractRatesSectionLocalIndex(NSIndexPath indexPath)
        {
            return (int)indexPath.Section - 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if( (int)section == 0 ) //Initial Terms
                return _numInitialPageCells;
            else if ((int) section < _contractModel.NumRates + 1) //Contract Rates
                return _numCellsPerRateSection;
            else if ((int)section < _contractModel.NumRates + 2) //Contacts
                return _numSendingPageCells;
            else if ((int)section < _contractModel.NumRates + 3)//Recipients
                return 0;
            else if ((int)section < _contractModel.NumRates + 4)//Associated POs
                return 0;
            else if ((int)section < _contractModel.NumRates + 5)//Email
                return 1;

            else return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 5 + _contractModel.NumRates;
        }
        /*
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return base.GetHeightForHeader(tableView, section);
        }
        */
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if ((int) section == 0)
                return "Contract Terms";
            else if ((int)section == 1)
                return "Contract Rate Term";
            else if ((int)section == _contractModel.NumRates + 1 )
                return "Contract Contacts";
            else if ((int)section == _contractModel.NumRates + 2)
                return "Invoice Recipients";
            else if ((int)section == _contractModel.NumRates + 3)
                return "Associated Project and POs to New Contract";
            else if ((int)section == _contractModel.NumRates + 4)
                return "Email";

            return string.Empty;
        }
    }
}
