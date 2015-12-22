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
	public class TimesheetContactsTableViewSource : UITableViewSource
	{
        private readonly TimesheetContactsTableViewController _parentController;
		private readonly TimesheetContactsTableViewModel _parentModel;
		private TimesheetContactsTableViewModel _tableModel;

		public TimesheetContactsTableViewSource
		(TimesheetContactsTableViewController parentController, TimesheetContact contact)
		{
			_parentController = parentController;
			_tableModel = new TimesheetContactsTableViewModel(contact);
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
			return _numContractorCells + _numDirectReportCells;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
		}

		private UITableViewCell GetEmailContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			if( IsContractorCell(indexPath) )
				cell.UpdateCell
				(
					_tableModel.ContractorEmailAddressByRowNumber((int)indexPath.Item - _numberOfContractorPhoneNumberCells), null
				);
			else
				cell.UpdateCell
				(
					_tableModel.DirectReportEmailAddressByRowNumber((int)indexPath.Item - _directReportDetailsIndex - _numberOfDirectReportPhoneNumberCells), null
				);
			
			return cell;
		}

		private UITableViewCell GetCallOrTextContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			if( IsContractorCell(indexPath) )
				cell.UpdateCell
				(
					null, _tableModel.ContractorPhoneNumberByRowNumber((int)indexPath.Item)
				);
			else
				cell.UpdateCell
				(
					null, _tableModel.DirectReportPhoneNumberByRowNumber((int)indexPath.Item)
				);

			return cell;
		}

		//private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
		//private bool IsSpecializationCell(NSIndexPath indexPath) { return (int)indexPath.Item == _specializationCellRow; }

		private int _firstContractorPhoneNumberCellIndex { get { return 1; } }
		private int _numberOfContractorPhoneNumberCells { get { return _tableModel.NumberOfContractorPhoneNumbers(); } }

		private int _firstDirectReportPhoneNumberCellIndex { get { return 1; } }
		private int _numberOfDirectReportPhoneNumberCells { get { return _tableModel.NumberOfDirectReportPhoneNumbers(); } }

		private bool IsCallOrTextCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item >= _firstContractorPhoneNumberCellIndex && (int)indexPath.Item < _firstContractorPhoneNumberCellIndex + _numberOfContractorPhoneNumberCells || 
				(int)indexPath.Item >= _firstDirectReportPhoneNumberCellIndex && (int)indexPath.Item < _firstDirectReportPhoneNumberCellIndex + _numberOfDirectReportPhoneNumberCells )
				return true;
			return false;
		}

		private bool IsContractorCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item < _firstDirectReportPhoneNumberCellIndex)
				return true;
			return false;
		}

		private int _firstContractorEmailCellIndex { get { return _firstContractorPhoneNumberCellIndex + _numberOfContractorPhoneNumberCells; } }
		private int _numberOfContractorEmailCells { get { return _tableModel.NumberOfContractorEmails(); } }

		private int _firstDirectReportEmailCellIndex { get { return _numberOfDirectReportPhoneNumberCells + _directReportDetailsIndex; } }
		private int _numberOfDirectReportEmailCells { get { return _tableModel.NumberOfDirectReportEmails(); } }

		private int _directReportDetailsIndex { get{ return _numContractorCells; } }
		private int _contractorDetailsIndex { get{ return 0; } }


		private bool IsEmailCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item >= _firstContractorEmailCellIndex && (int)indexPath.Item < _firstContractorEmailCellIndex + _numberOfContractorEmailCells || 
				(int)indexPath.Item >= _firstDirectReportEmailCellIndex && (int)indexPath.Item < _firstDirectReportEmailCellIndex + _numberOfDirectReportEmailCells)
				return true;
			return false;
		}

		private int _numContractorCells { get { return _numberOfContractorPhoneNumberCells + _numberOfContractorEmailCells + 1; } }
		private int _numDirectReportCells { get { return _numberOfDirectReportPhoneNumberCells + _numberOfDirectReportEmailCells + 1; } }


	}
}