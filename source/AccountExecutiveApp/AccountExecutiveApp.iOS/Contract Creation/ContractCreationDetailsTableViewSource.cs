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
	public class ContractCreationDetailsTableViewSource : UITableViewSource
	{
		private readonly ContractCreationDetailsTableViewController _parentController;
		private readonly ContractorDetailsTableViewModel _parentModel;
		private ContractorDetailsTableViewModel _tableModel;
		private float _specializationCellHeight = -1;

		public ContractCreationDetailsTableViewSource (ContractCreationDetailsTableViewController parentController)//, Contractor contractor)
		{
			_parentController = parentController;
			//_tableModel = new ContractorDetailsTableViewModel(contractor);
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
            
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);

            return cell;
		}


        private int _jobTitleCellRow { get { return 0; } }
        private bool IsJobTitleCell(NSIndexPath indexPath) { return (int)indexPath.Item == _jobTitleCellRow; }

        private int _startDateCellRow { get { return _jobTitleCellRow+1; } }
        private bool IsStartDateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _startDateCellRow; }

        private int _endDateCellRow { get { return _startDateCellRow + 1; } }
        private bool IsEndDateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _endDateCellRow; }

        private int _timeFactorCellRow { get { return _endDateCellRow + 1; } }
        private bool IsTimeFactorCell(NSIndexPath indexPath) { return (int)indexPath.Item == _timeFactorCellRow; }

        private int _daysCancellationCellRow { get { return _timeFactorCellRow + 1; } }
        private bool IsDaysCancellationCell(NSIndexPath indexPath) { return (int)indexPath.Item == _daysCancellationCellRow; }

        private int _limitationExpenseCellRow { get { return _daysCancellationCellRow + 1; } }
        private bool IsLimitationExpenseCell(NSIndexPath indexPath) { return (int)indexPath.Item == _limitationExpenseCellRow; }

        private int _limitationOfContractCellRow { get { return _limitationExpenseCellRow + 1; } }
        private bool IsLimitationOfContractCell(NSIndexPath indexPath) { return (int)indexPath.Item == _limitationOfContractCellRow; }

        private int _paymentPlanCellRow { get { return _limitationOfContractCellRow + 1; } }
        private bool IsPaymentPlanCell(NSIndexPath indexPath) { return (int)indexPath.Item == _paymentPlanCellRow; }

        private int _accountExecutiveCellRow { get { return _paymentPlanCellRow + 1; } }
        private bool IsAccountExecutiveCell(NSIndexPath indexPath) { return (int)indexPath.Item == _accountExecutiveCellRow; }

        private int _GMAssignedCellRow { get { return _accountExecutiveCellRow + 1; } }
        private bool IsGMAssignedCell(NSIndexPath indexPath) { return (int)indexPath.Item == _GMAssignedCellRow; }

        private int _comissionAssignedCellRow { get { return _GMAssignedCellRow + 1; } }
        private bool IsComissionAssignedCell(NSIndexPath indexPath) { return (int)indexPath.Item == _comissionAssignedCellRow; }

        private int _invoiceFrequencyCellRow { get { return _comissionAssignedCellRow + 1; } }
        private bool IsInvoiceFrequencyCell(NSIndexPath indexPath) { return (int)indexPath.Item == _invoiceFrequencyCellRow; }

        private int _invoiceFormatCellRow { get { return _invoiceFrequencyCellRow + 1; } }
        private bool IsInvoiceFormatCell(NSIndexPath indexPath) { return (int)indexPath.Item == _invoiceFormatCellRow; }

        private int _projectCodeCellRow { get { return _invoiceFormatCellRow + 1; } }
        private bool IsProjectCodeCell(NSIndexPath indexPath) { return (int)indexPath.Item == _projectCodeCellRow; }

        private int _quickPayCellRow { get { return _projectCodeCellRow + 1; } }
        private bool IsQuickPayCell(NSIndexPath indexPath) { return (int)indexPath.Item == _quickPayCellRow; }


	    private UITableViewCell GetJobTitleCell(UITableView tableView, NSIndexPath indexPath)
	    {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Job Title", "Developer");
	        return cell;
	    }
        private UITableViewCell GetStartDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell = (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Start Date", "08/02/2002");
            return cell;
        }
        private UITableViewCell GetEndDateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableDatePickerCell cell = (EditableDatePickerCell)tableView.DequeueReusableCell(EditableDatePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("End Date", "08/02/2003");
            return cell;
        }
        private UITableViewCell GetTimeFactorCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Time Factor", "Full Time");
            return cell;
        }
        private UITableViewCell GetDaysCancellationCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell = (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Days Cancellation", "10");
            return cell;
        }
        private UITableViewCell GetLimitationExpenseCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation Expense", "Regular");
            return cell;
        }
        private UITableViewCell GetLimitationOfContractCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Limitation of Contract", "????");
            return cell;
        }
        private UITableViewCell GetPaymentPlanCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Payment Plan", "Monthly Standard");
            return cell;
        }
        private UITableViewCell GetAccountExecutiveCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Account Executive", "Bob Smith");
            return cell;
        }
        private UITableViewCell GetGMAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("GM Assigned", "Bob Smith");
            return cell;
        }
        private UITableViewCell GetCommisionAssignedCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Comission Assigned", "Bob Smith"); 
            return cell;
        }
        private UITableViewCell GetInvoiceFrequencyCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Frequency", "Semi-Monthly");
            return cell;
        }
        private UITableViewCell GetInvoiceFormatCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell = (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Invoice Format", "1 invoice per contract"); 
            return cell;
        }
        private UITableViewCell GetProjectCodesCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell = (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Project/PO codes required", "Yes");
            return cell;
        }
        private UITableViewCell GetQuickPayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell = (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Quick Pay", "No");
            return cell;
        }

	    public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			UIView createContractHeader = new UIView ( new CGRect(0, 0, tableView.Frame.Width, 100));
			createContractHeader.BackgroundColor = UIColor.Cyan;
	        tableView.TableHeaderView = createContractHeader;
			return createContractHeader;
		}
        
		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 100.0f;
		}
        
		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 15;
			//return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1 + 2;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}
		/*
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if ((int)indexPath.Item == _specializationCellRow && _specializationCellHeight > 0)
				return _specializationCellHeight;
			return 44;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (IsContractsCell(indexPath))
			{
				ContractHistoryTableViewController vc =
					(ContractHistoryTableViewController)
					_parentController.Storyboard.InstantiateViewController("ContractHistoryTableViewController");

				vc.ShowSearchIcon = _parentController.ShowSearchIcon;
				vc.setContracts(_tableModel.Contracts);

				_parentController.ShowViewController(vc, _parentController);
			}
			if (IsResumeCell (indexPath)) 
			{
				ResumeViewController vc = (ResumeViewController) _parentController.Storyboard.InstantiateViewController("ResumeViewController");

				vc.Resume = _tableModel.ContractorResume;
				_parentController.ShowViewController(vc, _parentController);

			}
			if ( IsLinkedInCell(indexPath) )
			{

				NSUrl LinkedInWebUrl = NSUrl.FromString( _tableModel.LinkedInString );

				if (UIApplication.SharedApplication.CanOpenUrl(LinkedInWebUrl))
					UIApplication.SharedApplication.OpenUrl(LinkedInWebUrl);
				else
				{
					UIAlertController alertController = UIAlertController.Create("Error", "Could not launch LinkedIn at this time.",
						UIAlertControllerStyle.Alert);

					UIAlertAction okAction = UIAlertAction.Create("OK", UIAlertActionStyle.Default, delegate { alertController.DismissViewController(true, null);});
					alertController.AddAction(okAction);

					_parentController.PresentViewController( alertController, true, null);
				}

				var cell = tableView.CellAt(indexPath);
				cell.SetSelected(false, true);
			}
		}


		private void AddSpecializationAndSkills(IEnumerable<Specialization> specs, UITableViewCell cell)
		{
			var specFont = UIFont.SystemFontOfSize(17f);
			var skillFont = UIFont.SystemFontOfSize(14f);
			var frame = cell.Frame;
			var y = specs.Any() ? (int) specFont.LineHeight : 0;
			foreach (var spec in specs)
			{
				var specLabel = new UILabel
				{
					Text = spec.Name,
					Frame = new CGRect(20, y, frame.Width - 40, specFont.LineHeight),
					Font = specFont
				};
				cell.Add(specLabel);
				y += (int) specFont.LineHeight;
				var skillLabel = new UILabel
				{
					Text = GetSkillsString(spec.Skills),
					Frame = new CGRect(30, y, frame.Width - 50, skillFont.LineHeight),
					Font = skillFont,
					TextColor = StyleGuideConstants.DarkGrayUiColor,
					Lines = 0,
					LineBreakMode = UILineBreakMode.WordWrap
				};
				skillLabel.SizeToFit();
				y += (int) skillLabel.Frame.Height;
				cell.Add(skillLabel);
				y += (int) specFont.LineHeight;
			}
			frame.Height = y;
			_specializationCellHeight = (float) frame.Height;
		}

		private static string GetSkillsString(IEnumerable<Skill> skills)
		{
			var lines =
				skills.OrderByDescending(s => (int) s.YearsOfExperience)
					.ThenBy(s => s.Name)
					.Select(skill => string.Format("{0} {1}", skill.Name, skill.YearsOfExperience));

			return string.Join("\n", lines);
		}


		private UITableViewCell GetContractsCell(UITableView tableView)
		{
			var cell =
				tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
				RightDetailCell;

			cell.UpdateCell (
				mainText: "Contracts",
				rightDetailText: _tableModel.NumberOfContracts().ToString()
			);

			return cell;
		}

		private static UITableViewCell GetResumeCell(UITableView tableView)
		{
			var cell =
				tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
				RightDetailCell;

			cell.UpdateCell(
				mainText: "Resume",
				rightDetailText: ""
			);

			return cell;
		}

		private static UITableViewCell GetLinkedInCell(UITableView tableView)
		{
			var cell =
				tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
				RightDetailCell;

			UIImage LinkedInLogo = new UIImage("LinkedInLogo.png");
			UIImageView LinkedInImageView = new UIImageView(LinkedInLogo);
			LinkedInImageView.ContentMode = UIViewContentMode.Left;
			//cell.BackgroundView = LinkedInImageView;

			return cell;
		}


		private UITableViewCell GetSpecializationCell(UITableView tableView)
		{
			var cell = tableView.DequeueReusableCell("UITableViewCell");

			AddSpecializationAndSkills(_tableModel.Specializations, cell);

			return cell;
		}

		private UITableViewCell GetEmailContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			cell.UpdateCell
			(
				_tableModel.EmailAddressByRowNumber((int)indexPath.Item - _tableModel.NumberOfPhoneNumbers()), null
			);

			return cell;
		}

		private UITableViewCell GetCallOrTextContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			cell.UpdateCell
			(
				null, _tableModel.PhoneNumberByRowNumber((int)indexPath.Item)
			);

			return cell;
		}

		private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
		private bool IsSpecializationCell(NSIndexPath indexPath){ return (int) indexPath.Item == _specializationCellRow; }

		private int _LinkedInCellRow { get { return _specializationCellRow + 1; } }
		private bool IsLinkedInCell(NSIndexPath indexPath) { return (int)indexPath.Item == _LinkedInCellRow; }

		private int _resumeCellRow { get { return _LinkedInCellRow + 1; } }
		private bool IsResumeCell(NSIndexPath indexPath) { return (int)indexPath.Item == _resumeCellRow; }

		private int _contractsCellRow { get { return _resumeCellRow + 1; } }
		private bool IsContractsCell(NSIndexPath indexPath) { return (int)indexPath.Item == _contractsCellRow; }

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
		}*/
	}
}