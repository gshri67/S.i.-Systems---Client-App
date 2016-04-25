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
	    private MatchGuideConstants.TimesheetStatus _status;

		public TimesheetContactsTableViewSource
		(TimesheetContactsTableViewController parentController, TimesheetContact contact, MatchGuideConstants.TimesheetStatus status)
		{
			_parentController = parentController;
			_tableModel = new TimesheetContactsTableViewModel(contact);
		    _status = status;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
		    if (IsDetailsCell(indexPath))
		        return GetDetailsCell( tableView, indexPath );
			if (IsCallOrTextCell(indexPath))
				return GetCallOrTextContactCell(tableView, indexPath);
			else if (IsEmailCell(indexPath))
				return GetEmailContactCell(tableView, indexPath);
            else if (IsRequestButtonCell(indexPath))
                return GetRequestButtonCell(tableView, indexPath);


			return null;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
		    return _numContractorCells + _numDirectReportCells + _numClientContactCells;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

	    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
	    {
            if (IsDetailsCell(indexPath) && IsContractorSubCell(indexPath))
	        {
                var vc = (ContractorDetailsTableViewController)_parentController.Storyboard.InstantiateViewController("ContractorDetailsTableViewController");
                vc.setContractorId(_tableModel.ContractorId);
                _parentController.ShowViewController(vc, _parentController);
	        }
            else if (IsDetailsCell(indexPath) && IsDirectReportSubCell(indexPath))
            {
                var vc = (ClientContactDetailsViewController)_parentController.Storyboard.InstantiateViewController("ClientContactDetailsViewController");
                vc.SetContactId(_tableModel.DirectReportId, UserContactType.DirectReport);
                _parentController.ShowViewController(vc, _parentController);
            }
            else if (IsDetailsCell(indexPath) && IsClientContactSubCell(indexPath))
            {
                var vc = (ClientContactDetailsViewController)_parentController.Storyboard.InstantiateViewController("ClientContactDetailsViewController");
                vc.SetContactId(_tableModel.ClientContactId, UserContactType.ClientContact);
                _parentController.ShowViewController(vc, _parentController);
            }
	    }

	    private UITableViewCell GetRequestButtonCell(UITableView tableView, NSIndexPath indexPath)
	    {
	        var cell = tableView.DequeueReusableCell(ButtonCell.CellIdentifier) as ButtonCell;
           
            cell.UpdateCell("Re-Request Approval");

	        cell.OnButtonTapped = delegate
	        {
	            _parentController.RequestTimesheetApproval();
	        };

	        return cell;
	    }

	    private UITableViewCell GetDetailsCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(SubtitleWithRightDetailCell.CellIdentifier) as
                SubtitleWithRightDetailCell;

            if (IsContractorSubCell(indexPath))
                cell.UpdateCell
                (
                    mainText: _tableModel.ContractorFullName, 
                    subtitleText: "Contractor",
                    rightDetailText: "" 
                );
            else if (IsDirectReportSubCell(indexPath))
                cell.UpdateCell
                (
                    mainText: _tableModel.DirectReportFullName,
                    subtitleText: "Direct Report",
                    rightDetailText: ""
                );
            else if (IsClientContactSubCell(indexPath))
                cell.UpdateCell
                (
                    mainText: _tableModel.ClientContactFullName,
                    subtitleText: "Client Contact",
                    rightDetailText: ""
                );

            return cell;
        }

		private UITableViewCell GetEmailContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			if( IsContractorSubCell(indexPath) )
				cell.UpdateCell
				(
					_tableModel.ContractorEmailAddressByRowNumber((int)indexPath.Item - _firstContractorEmailCellIndex), null
				);
            else if (IsDirectReportSubCell(indexPath))
				cell.UpdateCell
				(
                    _tableModel.DirectReportEmailAddressByRowNumber((int)indexPath.Item - _firstDirectReportEmailCellIndex), null
				);
            else if (IsClientContactSubCell(indexPath))
                cell.UpdateCell
                (
                    _tableModel.ClientContactEmailAddressByRowNumber((int)indexPath.Item - _firstClientContactEmailCellIndex), null
                );
			return cell;
		}

		private UITableViewCell GetCallOrTextContactCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell =
				tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
				ContractorContactInfoCell;

			cell.ParentViewController = _parentController;

			if( IsContractorSubCell(indexPath) )
				cell.UpdateCell
				(
                    null, _tableModel.ContractorPhoneNumberByRowNumber((int)indexPath.Item - _firstContractorPhoneNumberCellIndex)
				);
            else if (IsDirectReportSubCell(indexPath))
				cell.UpdateCell
				(
                    null, _tableModel.DirectReportPhoneNumberByRowNumber((int)indexPath.Item - _firstDirectReportPhoneNumberCellIndex)
				);
            else if (IsClientContactSubCell(indexPath))
                cell.UpdateCell
                (
                    null, _tableModel.ClientContactPhoneNumberByRowNumber((int)indexPath.Item - _firstClientContactPhoneNumberCellIndex)
                );

			return cell;
		}

		//private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
		//private bool IsSpecializationCell(NSIndexPath indexPath) { return (int)indexPath.Item == _specializationCellRow; }

		private int _firstContractorPhoneNumberCellIndex { get { return _contractorDetailsIndex+1; } }
		private int _numberOfContractorPhoneNumberCells { get { return _tableModel.NumberOfContractorPhoneNumbers(); } }

        private int _requestButtonCellIndex { get { return _directReportDetailsIndex + 1; } }

        private bool IsRequestButtonCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item == _requestButtonCellIndex)
                return true;
            return false;
        }

        private int _firstDirectReportPhoneNumberCellIndex { 
            get
            {
                if (_status == MatchGuideConstants.TimesheetStatus.Submitted) return _requestButtonCellIndex + 1;
                else return _directReportDetailsIndex + 1;
            } 
        }
		private int _numberOfDirectReportPhoneNumberCells { get { return _tableModel.NumberOfDirectReportPhoneNumbers(); } }


        private int _firstClientContactPhoneNumberCellIndex{ get { return _clientContactDetailsIndex + 1; } }
        private int _numberOfClientContactPhoneNumberCells { get { return _tableModel.NumberOfClientContactPhoneNumbers(); } }



		private bool IsCallOrTextCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item >= _firstContractorPhoneNumberCellIndex && (int)indexPath.Item < _firstContractorPhoneNumberCellIndex + _numberOfContractorPhoneNumberCells ||
                (int)indexPath.Item >= _firstDirectReportPhoneNumberCellIndex && (int)indexPath.Item < _firstDirectReportPhoneNumberCellIndex + _numberOfDirectReportPhoneNumberCells ||
                (int)indexPath.Item >= _firstClientContactPhoneNumberCellIndex && (int)indexPath.Item < _firstClientContactPhoneNumberCellIndex + _numberOfClientContactPhoneNumberCells)
				    return true;

			return false;
		}

		private bool IsContractorSubCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item < _directReportDetailsIndex)
				return true;
			return false;
		}

        private bool IsDirectReportSubCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _directReportDetailsIndex && !IsClientContactSubCell(indexPath) )
                return true;

            return false;
        }
        private bool IsClientContactSubCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _clientContactDetailsIndex)
                return true;

            return false;
        }

        private bool IsDetailsCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item == _directReportDetailsIndex || (int)indexPath.Item == _clientContactDetailsIndex || (int)indexPath.Item == _contractorDetailsIndex)
                return true;

            return false;
        }

		private int _firstContractorEmailCellIndex { get { return _firstContractorPhoneNumberCellIndex + _numberOfContractorPhoneNumberCells; } }
		private int _numberOfContractorEmailCells { get { return _tableModel.NumberOfContractorEmails(); } }

        private int _firstDirectReportEmailCellIndex { get { return _firstDirectReportPhoneNumberCellIndex + _numberOfDirectReportPhoneNumberCells; } }
		private int _numberOfDirectReportEmailCells { get { return _tableModel.NumberOfDirectReportEmails(); } }

        private int _firstClientContactEmailCellIndex { get { return _firstClientContactPhoneNumberCellIndex + _numberOfClientContactPhoneNumberCells; } }
        private int _numberOfClientContactEmailCells { get { return _tableModel.NumberOfClientContactEmails(); } }

		private int _directReportDetailsIndex { get{ return _numContractorCells; } }
        private int _clientContactDetailsIndex { get { return _numContractorCells + _numDirectReportCells; } }
		private int _contractorDetailsIndex { get{ return 0; } }


		private bool IsEmailCell(NSIndexPath indexPath)
		{
			if ((int)indexPath.Item >= _firstContractorEmailCellIndex && (int)indexPath.Item < _firstContractorEmailCellIndex + _numberOfContractorEmailCells ||
                (int)indexPath.Item >= _firstDirectReportEmailCellIndex && (int)indexPath.Item < _firstDirectReportEmailCellIndex + _numberOfDirectReportEmailCells ||
                (int)indexPath.Item >= _firstClientContactEmailCellIndex && (int)indexPath.Item < _firstClientContactEmailCellIndex + _numberOfClientContactEmailCells)
				return true;
			return false;
		}

		private int _numContractorCells { get { return _numberOfContractorPhoneNumberCells + _numberOfContractorEmailCells + 1; } }
		private int _numDirectReportCells { 
            get
		    {
                if( _status == MatchGuideConstants.TimesheetStatus.Submitted )
    		        return _numberOfDirectReportPhoneNumberCells + _numberOfDirectReportEmailCells + 2;
                else
                    return _numberOfDirectReportPhoneNumberCells + _numberOfDirectReportEmailCells + 1;
		    }
        }
        private int _numClientContactCells { get { return _numberOfClientContactPhoneNumberCells + _numberOfClientContactEmailCells + 1; } }

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if ( IsDetailsCell(indexPath) )
				return 65;
			return 44;
		}
	}
}