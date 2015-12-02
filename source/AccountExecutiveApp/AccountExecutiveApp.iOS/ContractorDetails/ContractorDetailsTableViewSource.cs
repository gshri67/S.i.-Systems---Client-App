using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ContractorDetailsTableViewSource : UITableViewSource
    {
        private readonly ContractorDetailsTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;
        private IEnumerable<string> PhoneNumbers;
        private IEnumerable<string> Emails;
 

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController)
        {
            _parentController = parentController;
            //_parentModel = parentModel;

            PhoneNumbers = new List<String>{"", ""}.AsEnumerable();
            Emails = new List<String> { "" }.AsEnumerable();


        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if ((int) indexPath.Item < PhoneNumbers.Count() + Emails.Count())
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
            else if ((int) indexPath.Item == PhoneNumbers.Count() + Emails.Count())
            {
                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.UpdateCell
                (
                    mainContactText: "Specialization Cell",
                    contactTypeText: "Subtitle",
                    canPhone: true,
                    canText: true,
                    canEmail: false
                );

                return cell;
            }
            else if ((int) indexPath.Item == PhoneNumbers.Count() + Emails.Count()+1)
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Resume";

                return cell;
            }
            else if ((int)indexPath.Item == PhoneNumbers.Count() + Emails.Count() + 2)
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Contracts";
                cell.DetailTextLabel.Text = "3 Contracts";

                return cell;
            }

            return null;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return PhoneNumbers.Count() + Emails.Count() + 1 + 2;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}