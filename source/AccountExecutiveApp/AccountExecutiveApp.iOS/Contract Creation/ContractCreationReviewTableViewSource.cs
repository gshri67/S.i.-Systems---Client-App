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
            UITableViewCell cell;

            if (indexPath.Section == 0)
                cell = ContractBodyCell(tableView, indexPath);
            else if (indexPath.Section < _contractModel.NumRates + 1)
                cell = RatesCell(tableView, indexPath);
            else if (indexPath.Section < _contractModel.NumRates + 2)
                cell = ContactsCell(tableView, indexPath);
            else if (indexPath.Section < _contractModel.NumRates + 3)//Recipients
                cell = InvoiceRecipientsCell(tableView, indexPath);
            else if (indexPath.Section < _contractModel.NumRates + 4)//Associated POs
                cell = null;
            else if (indexPath.Section < _contractModel.NumRates + 5) //Email
                cell = ContractEmailCell(tableView, indexPath);
            else
                cell = new UITableViewCell();

            cell.UserInteractionEnabled = false;

            return cell;
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

        private UITableViewCell ContractEmailCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _isSendingConsultantContractCellRow))
                return GetIsSendingConsultantContractCell(tableView, indexPath);
            if ( _showEmailCell && IsIndexFromCell(indexPath, _emailCellRow))
                return GetEmailCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _clientContractCellRow))
                return GetClientContractCell(tableView, indexPath);
            if ( _showClientContractCellReason && IsIndexFromCell(indexPath, _reasonCellRow))
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

        private UITableViewCell InvoiceRecipientsCell(UITableView tableView, NSIndexPath indexPath)
        {
            return GetInvoiceRecipientsCell(tableView, indexPath);
        }

        private UITableViewCell RatesCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsIndexFromCell(indexPath, _localRateTypeCellRow))
                return GetRateTypeCell(tableView, indexPath);
            else if ( ShowHoursPerDay && IsIndexFromCell(indexPath, _localHoursPerDayCellRow))
                return GetHoursPerDayCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localRateDescriptionCellRow))
                return GetRateDescriptionCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localBillRateCellRow))
                return GetBillRateCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localPayRateCellRow))
                return GetPayRateCell(tableView, indexPath);
            if (IsIndexFromCell(indexPath, _localGrossMarginCellRow))
                return GetGrossMarginCell(tableView, indexPath);

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


        private bool ShowHoursPerDay
        {
            get { return _contractModel.AreRateTypesPerDay; }
        }

        private int _localRateTypeCellRow { get { return 0; } }

        private int _localHoursPerDayCellRow { get { return _localRateTypeCellRow + 1; } }

        private int _localRateDescriptionCellRow { get { if (!ShowHoursPerDay) return _localRateTypeCellRow + 1; return _localHoursPerDayCellRow + 1; } }

        private int _localBillRateCellRow { get { return _localRateDescriptionCellRow + 1; } }

        private int _localPayRateCellRow { get { return _localBillRateCellRow + 1; } }

        private int _localGrossMarginCellRow { get { return _localPayRateCellRow + 1; } }

        private int _localIsPrimaryRateCellRow { get { return _localGrossMarginCellRow + 1; } }

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
            
            return cell;
        }

        private UITableViewCell GetStartDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Start Date", _contractModel.FormattedStartDate);
          
            return cell;
        }

        private UITableViewCell GetEndDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell =
                (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("End Date", _contractModel.FormattedEndDate);
            
            return cell;
        }

        private UITableViewCell GetTimeFactorCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Time Factor", _contractModel.TimeFactor);
            
            return cell;
        }

        private UITableViewCell GetDaysCancellationCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Days Cancellation", _contractModel.DaysCancellation.ToString());

            return cell;
        }

        private UITableViewCell GetLimitationExpenseCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation Expense", _contractModel.LimitationExpense);

            return cell;
        }

        private UITableViewCell GetLimitationOfContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation of Contract", _contractModel.LimitationOfContractType);

            return cell;
        }

        private UITableViewCell GetPaymentPlanCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Payment Plan", _contractModel.PaymentPlan);

            return cell;
        }

        private UITableViewCell GetAccountExecutiveCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Account Executive", _contractModel.AccountExecutive.FullName);

            return cell;
        }

        private UITableViewCell GetGMAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("GM Assigned", _contractModel.GMAssigned.FullName);

            return cell;
        }

        private UITableViewCell GetCommisionAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Comission Assigned", _contractModel.ComissionAssigned.FullName);

            return cell;
        }

        private UITableViewCell GetInvoiceFrequencyCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Frequency", _contractModel.InvoiceFrequency);

            return cell;
        }

        private UITableViewCell GetInvoiceFormatCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Format", _contractModel.InvoiceFormat);

            return cell;
        }

        private UITableViewCell GetProjectCodesCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Project/PO codes required", _contractModel.UsingProjectCode);
      
            return cell;
        }

        private UITableViewCell GetQuickPayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Quick Pay", _contractModel.UsingQuickPay);
        
            return cell;
        }


//Get Cells for Contract Rates
        private UITableViewCell GetRateTypeCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Rate Type", _contractModel.RateTypeAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }

        private UITableViewCell GetHoursPerDayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);

            cell.UpdateCell("Hours", _contractModel.HoursPerDayAtIndex(ContractRatesSectionLocalIndex(indexPath)).ToString());

            return cell;
        }

        private UITableViewCell GetRateDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Rate Description", _contractModel.RateDescriptionAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }

        private UITableViewCell GetBillRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Bill Rate", _contractModel.BillRateAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }

        private UITableViewCell GetPayRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Pay Rate", _contractModel.PayRateAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }

        private UITableViewCell GetGrossMarginCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);

            cell.UpdateCell("GM", _contractModel.GrossMarginAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }

        private UITableViewCell GetIsPrimaryRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Primary Rate", _contractModel.IsPrimaryRateAtIndex(ContractRatesSectionLocalIndex(indexPath)));

            return cell;
        }
      
//Get Sending Page cells
        private UITableViewCell GetClientContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Client Contact", _contractModel.ClientContactName);

            return cell;
        }

        private UITableViewCell GetDirectReportCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Direct Report", _contractModel.DirectReportName);

            return cell;
        }

        private UITableViewCell GetBillingContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Billing Contact", _contractModel.BillingContactName);

            return cell;
        }

        private UITableViewCell GetInvoiceRecipientsCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MultiSelectDescriptionCell)tableView.DequeueReusableCell(MultiSelectDescriptionCell.CellIdentifier, indexPath);

            cell.UpdateCell("Invoice Recipients", _contractModel.Contract.InvoiceRecipients.Select(c => c.FullName).ToList());

            return cell;
        }

//Get Email Section Cells
        private UITableViewCell GetIsSendingConsultantContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell(string.Format("Send consultant e-contract to {0}:", _contractModel.ConsultantName), _contractModel.IsSendingConsultantContract);

            return cell;
        }

        private UITableViewCell GetEmailCell(UITableView tableView, NSIndexPath indexPath)
        {
            EmailCell cell = (EmailCell)tableView.DequeueReusableCell(EmailCell.CellIdentifier, indexPath);
            cell.UpdateCell("Agreement Between Bob Smith and Cenovus about the immediate sharepoint job opportunity", "Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...Dear Jean-Claude, You are invited by S.i. Systems...");

            return cell;
        }

        private UITableViewCell GetClientContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (EditableDoubleTextFieldCell)tableView.DequeueReusableCell(EditableDoubleTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell(string.Format("Send Client e-contract to:"), _contractModel.ClientContractContactName, _contractModel.IsSendingContractToClientContact);

            return cell;
        }

        private UITableViewCell GetReasonCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Reason:", _contractModel.ReasonForNotSendingContract);

            return cell;
        }

        private UITableViewCell GetOtherReasonCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableFullTextFieldCell cell = (EditableFullTextFieldCell)tableView.DequeueReusableCell(EditableFullTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Reason:", _contractModel.SummaryReasonForNotSendingContract);

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
