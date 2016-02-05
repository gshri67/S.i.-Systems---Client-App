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
    public class ClientContactDetailsTableViewSource : UITableViewSource
    {
        private readonly ClientContactDetailsViewController _parentController;
        private readonly ClientContactDetailsTableViewModel _parentModel;
        private ClientContactDetailsTableViewModel _tableModel;

        public ClientContactDetailsTableViewSource(ClientContactDetailsViewController parentController, UserContact ClientContact)
        {
            _parentController = parentController;
            _tableModel = new ClientContactDetailsTableViewModel(ClientContact);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsLinkedInCell(indexPath))
                return GetLinkedInCell(tableView);
            else if (IsCallOrTextCell(indexPath))
                return GetCallOrTextContactCell(tableView, indexPath);
            else if (IsEmailCell(indexPath))
                return GetEmailContactCell(tableView, indexPath);

            return null;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsLinkedInCell(indexPath))
            {
                NSUrl LinkedInWebUrl = NSUrl.FromString(_tableModel.LinkedInString);

                if (UIApplication.SharedApplication.CanOpenUrl(LinkedInWebUrl))
                    UIApplication.SharedApplication.OpenUrl(LinkedInWebUrl);
                else
                {
                    UIAlertController alertController = UIAlertController.Create("Error", "Could not launch LinkedIn at this time.",
                        UIAlertControllerStyle.Alert);

                    UIAlertAction okAction = UIAlertAction.Create("OK", UIAlertActionStyle.Default, delegate { alertController.DismissViewController(true, null); });
                    alertController.AddAction(okAction);

                    _parentController.PresentViewController(alertController, true, null);
                }

                var cell = tableView.CellAt(indexPath);
                cell.SetSelected(false, true);
            }
        }

        private static UITableViewCell GetLinkedInCell(UITableView tableView)
        {
            var cell =
                tableView.DequeueReusableCell(LinkedInSearchCell.CellIdentifier) as
                    LinkedInSearchCell;

            return cell;
        }

        private UITableViewCell GetEmailContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
                    ContractorContactInfoCell;

            cell.ParentViewController = _parentController;

            cell.UpdateCell
                (
                    _tableModel.EmailAddressByRowNumber((int)indexPath.Item - _firstEmailCellIndex), null
                );

            return cell;
        }

        private UITableViewCell GetCallOrTextContactCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(ClientContactDetailsViewController.CellIdentifier) as
                    ContractorContactInfoCell;

            cell.ParentViewController = _parentController;

            cell.UpdateCell
            (
                null, _tableModel.PhoneNumberByRowNumber((int)indexPath.Item - _firstPhoneNumberCellIndex)
            );

            return cell;
        }

        //private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
        //private bool IsSpecializationCell(NSIndexPath indexPath) { return (int)indexPath.Item == _specializationCellRow; }

        private int _LinkedInCellRow { get { return 0; } }
        private bool IsLinkedInCell(NSIndexPath indexPath) { return (int)indexPath.Item == _LinkedInCellRow; }

        private int _firstPhoneNumberCellIndex { get { return _LinkedInCellRow + 1; } }
        private int _numberOfPhoneNumberCells { get { return _tableModel.NumberOfPhoneNumbers(); } }
        private bool IsCallOrTextCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _firstPhoneNumberCellIndex && (int)indexPath.Item < _firstPhoneNumberCellIndex + _numberOfPhoneNumberCells)
                return true;
            return false;
        }

        private int _firstEmailCellIndex { get { return _numberOfPhoneNumberCells + _firstPhoneNumberCellIndex; } }
        private int _numberOfEmailCells { get { return _tableModel.NumberOfEmails(); } }
        private bool IsEmailCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _firstEmailCellIndex && (int)indexPath.Item < _firstEmailCellIndex + _numberOfEmailCells)
                return true;
            return false;
        }
    }
}