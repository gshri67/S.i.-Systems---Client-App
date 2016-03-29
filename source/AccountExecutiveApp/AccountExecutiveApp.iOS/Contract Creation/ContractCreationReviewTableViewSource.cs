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

        private UITableViewCell ContractBodyCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _jobTitleCellRow))
                return GetJobTitleCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _startDateCellRow))
                return GetStartDateCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _endDateCellRow))
                return GetEndDateCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _timeFactorCellRow))
                return GetTimeFactorCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _daysCancellationCellRow))
                return GetDaysCancellationCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _limitationExpenseCellRow))
                return GetLimitationExpenseCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _limitationOfContractCellRow))
                return GetLimitationOfContractCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _paymentPlanCellRow))
                return GetPaymentPlanCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _accountExecutiveCellRow))
                return GetAccountExecutiveCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _GMAssignedCellRow))
                return GetGMAssignedCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _comissionAssignedCellRow))
                return GetCommisionAssignedCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _invoiceFrequencyCellRow)) 
                return GetInvoiceFrequencyCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _invoiceFormatCellRow))
                return GetInvoiceFormatCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _projectCodeCellRow))
                return GetProjectCodesCell(tableView, indexPath);
            
            return GetQuickPayCell(tableView, indexPath);

        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
                return ContractBodyCell(tableView, indexPath);
            if (indexPath.Section < _contractModel.NumRates + 1)
                return RatesCell(tableView, indexPath);
            if (indexPath.Section < _contractModel.NumRates + 2)
                return ContactsCell(tableView, indexPath);
            if (indexPath.Section < _contractModel.NumRates + 3)//Recipients
                return null;
            if (indexPath.Section < _contractModel.NumRates + 4)//Associated POs
                return null;
            if (indexPath.Section < _contractModel.NumRates + 5) //Email
                return ContractEmailCell(tableView, indexPath);
            return (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
        }

        private UITableViewCell ContractEmailCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _isSendingConsultantContractCellRow))
                return GetIsSendingConsultantContractCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _emailCellRow))
                return GetEmailCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _clientContractCellRow))
                return GetClientContractCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _reasonCellRow))
                return GetReasonCell(tableView, indexPath);
            
            return GetOtherReasonCell(tableView, indexPath);
        }

        private UITableViewCell ContactsCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _clientContactCellRow))
                return GetClientContactCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _directReportCellRow))
                return GetDirectReportCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _billingContactCellRow))
                return GetBillingContactCell(tableView, indexPath);

            return GetInvoiceRecipientsCell(tableView, indexPath);
        }

        private UITableViewCell RatesCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _localRateTypeCellRow))
                return GetRateTypeCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localRateDescriptionCellRow))
                return GetRateDescriptionCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localBillRateCellRow))
                return GetBillRateCell(tableView, indexPath);

            return GetIsPrimaryRateCell(tableView, indexPath);
        }

        private static bool IsIndexFromCell(NSIndexPath indexPath, int cellRow)
        {
            return (int)indexPath.Item == cellRow;
        }

//Initial (Body) Page Indices

        private int _jobTitleCellRow
        {
            get { return 0; }
        }

        private int _startDateCellRow
        {
            get { return _jobTitleCellRow + 1; }
        }

        private int _endDateCellRow
        {
            get { return _startDateCellRow + 1; }
        }

        private int _timeFactorCellRow
        {
            get { return _endDateCellRow + 1; }
        }


        private int _daysCancellationCellRow
        {
            get { return _timeFactorCellRow + 1; }
        }

        private int _limitationExpenseCellRow
        {
            get { return _daysCancellationCellRow + 1; }
        }

        private int _limitationOfContractCellRow
        {
            get { return _limitationExpenseCellRow + 1; }
        }


        private int _paymentPlanCellRow
        {
            get { return _limitationOfContractCellRow + 1; }
        }


        private int _accountExecutiveCellRow
        {
            get { return _paymentPlanCellRow + 1; }
        }


        private int _GMAssignedCellRow
        {
            get { return _accountExecutiveCellRow + 1; }
        }

        private int _comissionAssignedCellRow
        {
            get { return _GMAssignedCellRow + 1; }
        }

        private int _invoiceFrequencyCellRow
        {
            get { return _comissionAssignedCellRow + 1; }
        }

        private int _invoiceFormatCellRow
        {
            get { return _invoiceFrequencyCellRow + 1; }
        }

        private int _projectCodeCellRow
        {
            get { return _invoiceFormatCellRow + 1; }
        }

        private int _quickPayCellRow
        {
            get { return _projectCodeCellRow + 1; }
        }


        private int _numInitialPageCells
        {
            get { return _quickPayCellRow + 1; }
        }

//Contract Rates Page Indices

        private int _localRateTypeCellRow { get { return 0; } }

        private int _localRateDescriptionCellRow { get { return _localRateTypeCellRow + 1; } }

        private int _localBillRateCellRow { get { return _localRateDescriptionCellRow + 1; } }

        private int _localIsPrimaryRateCellRow { get { return _localBillRateCellRow + 1; } }

        private int _numCellsPerRateSection { get { return _localIsPrimaryRateCellRow + 1; } }

//Sending Page Indices

        private int _clientContactCellRow
        {
            get { return 0; }
        }

        private int _directReportCellRow
        {
            get { return _clientContactCellRow + 1; }
        }

        private int _billingContactCellRow
        {
            get { return _directReportCellRow + 1; }
        }

        private int _invoiceRecipientsCellRow
        {
            get { return _billingContactCellRow + 1; }
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

        private int _emailCellRow
        {
            get { return _isSendingConsultantContractCellRow+1; }
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

        private int _reasonCellRow
        {
            get { return _clientContractCellRow + 1; }
        }

        private int _otherReasonCellRow
        {
            get { return _reasonCellRow + 1; }
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
            cell.UpdateCell("Agreement Between .. ..", "Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...");

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
            return indexPath.Section - 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if( (int)section == 0 ) //Initial Terms
                return _numInitialPageCells;
            if ((int) section < _contractModel.NumRates + 1) //Contract Rates
                return _numCellsPerRateSection;
            if ((int)section < _contractModel.NumRates + 2) //Contacts
                return _numSendingPageCells;
            if ((int)section < _contractModel.NumRates + 3)//Recipients
                return 0;
            if ((int)section < _contractModel.NumRates + 4)//Associated POs
                return 0;
            if ((int)section < _contractModel.NumRates + 5)//Email
                return _numEmailPageCells;

            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 5 + _contractModel.NumRates;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            if ((int) section == 0)
                return "Contract Terms";
            if ((int)section == 1)
                return "Contract Rate Term";
            if ((int)section == _contractModel.NumRates + 1 )
                return "Contract Contacts";
            if ((int)section == _contractModel.NumRates + 2)
                return "Invoice Recipients";
            if ((int)section == _contractModel.NumRates + 3)
                return "Associated Project and POs to New Contract";
            if ((int)section == _contractModel.NumRates + 4)
                return "Email";

            return string.Empty;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 50;
        }
    }
}
