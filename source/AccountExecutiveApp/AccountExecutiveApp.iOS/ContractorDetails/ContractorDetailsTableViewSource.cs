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
    public class ContractorDetailsTableViewSource : UITableViewSource
    {
        private readonly ContractorDetailsTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;
        private ContractorDetailsTableViewModel _tableModel;
        private float _specializationCellHeight = -1;

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController,
            Contractor contractor)
        {
            _parentController = parentController;
            //_parentModel = parentModel;

            _tableModel = new ContractorDetailsTableViewModel(contractor);

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if ((int) indexPath.Item < _tableModel.NumberOfPhoneNumbers())
            {

                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.ParentViewController = _parentController;

                cell.UpdateCell
                    (
                        mainContactText: _tableModel.FormattedPhoneNumberByRowNumber((int) indexPath.Item),
                        contactTypeText: "Mobile",
                        canPhone: true,
                        canText: true,
                        canEmail: false
                    );

                return cell;
            }
            if ((int) indexPath.Item < _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails())
            {

                var cell =
                    tableView.DequeueReusableCell(ContractorDetailsTableViewController.CellIdentifier) as
                        ContractorContactInfoCell;

                cell.ParentViewController = _parentController;

                cell.UpdateCell
                    (
                        mainContactText:
                            _tableModel.FormattedEmailByRowNumber((int) indexPath.Item -
                                                                  _tableModel.NumberOfPhoneNumbers()),
                        contactTypeText: "Home",
                        canPhone: false,
                        canText: false,
                        canEmail: true
                    );

                return cell;
            }
            else if ((int) indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails())
            {
                /*
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Specialization";

                return cell;
                 */

                var cell = tableView.DequeueReusableCell("UITableViewCell");

                AddSpecializationAndSkills(_tableModel.Specializations, cell);

                return cell;
            }
            else if ((int) indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 1)
            {
                var cell =
                    tableView.DequeueReusableCell(RightDetailCell.CellIdentifier) as
                        RightDetailCell;

                cell.TextLabel.Text = "Resume";

                return cell;
            }
            else if ((int) indexPath.Item == _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails() + 2)
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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if ((int) indexPath.Item == _specializationCellRow && _specializationCellHeight > 0)
                return _specializationCellHeight;
            return 44;
        }

        private int _specializationCellRow { get { return _tableModel.NumberOfPhoneNumbers() + _tableModel.NumberOfEmails(); } }
        private bool IsSpecializationCellRow(NSIndexPath indexPath)
        {
            if ((int) indexPath.Item == _specializationCellRow)
                return true;
            return false;
        }

        private int _firstPhoneNumberCellIndex { get { return 0; } }
        private int _numberOfPhoneNumberCells { get { return _tableModel.NumberOfPhoneNumbers(); } }
        private bool IsCallOrTextCell(NSIndexPath indexPath)
        {
            if ((int)indexPath.Item >= _firstPhoneNumberCellIndex && (int)indexPath.Item < _firstPhoneNumberCellIndex + _numberOfPhoneNumberCells)
                return true;
            return false;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
        }
    }
}