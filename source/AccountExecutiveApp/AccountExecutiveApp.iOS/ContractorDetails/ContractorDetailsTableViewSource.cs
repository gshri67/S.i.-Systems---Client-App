using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ContractorDetailsTableViewSource : UITableViewSource
    {
        private readonly ContractorDetailsTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;
        private ContractorDetailsTableViewModel _tableModel;

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController, Contractor contractor)
        {
            _parentController = parentController;
            //_parentModel = parentModel;

            _tableModel = new ContractorDetailsTableViewModel(contractor);

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if ((int)indexPath.Item < _tableModel.NumberOfPhoneNumbers())
            {
            
                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.UpdateCell
                (
                    mainContactText: _tableModel.FormattedPhoneNumberByRowNumber((int)indexPath.Item),
                    contactTypeText: "Mobile",
                    canPhone: true,
                    canText: true,
                    canEmail: false
                );

                return cell;
            }
            if ((int)indexPath.Item < _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails())
            {

                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.UpdateCell
                (
                    mainContactText: _tableModel.FormattedEmailByRowNumber((int)indexPath.Item - _tableModel.NumberOfPhoneNumbers()),
                    contactTypeText: "Home",
                    canPhone: false,
                    canText: false,
                    canEmail: true
                );

                return cell;
            }
            else if ((int)indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails())
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Specialization";

                return cell;
            }
            else if ((int)indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1)
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Resume";

                return cell;
            }
            else if ((int)indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 2)
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Contracts";
                cell.DetailTextLabel.Text = _tableModel.NumberOfContracts().ToString();

                return cell;
            }

            return null;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1 + 2;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}