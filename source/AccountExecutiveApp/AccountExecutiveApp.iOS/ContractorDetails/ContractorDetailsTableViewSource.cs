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
        private IEnumerable<string> PhoneNumbers;
        private IEnumerable<string> Emails;
        private ContractorDetailsTableViewModel _tableModel;

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController, Consultant consultant)
        {
            _parentController = parentController;
            //_parentModel = parentModel;

            PhoneNumbers = new List<String>{"", ""}.AsEnumerable();
            Emails = new List<String> { "" }.AsEnumerable();
            _tableModel = new ContractorDetailsTableViewModel(consultant);

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if ((int)indexPath.Item < _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails())
            {
            
                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.UpdateCell
                (
                    mainContactText: "(403) 222-8818",
                    contactTypeText: "Subtitle",
                    canPhone: true,
                    canText: true,
                    canEmail: false
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