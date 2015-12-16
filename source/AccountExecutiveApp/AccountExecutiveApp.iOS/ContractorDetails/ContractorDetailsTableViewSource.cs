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

        public ContractorDetailsTableViewSource(ContractorDetailsTableViewController parentController, Contractor contractor)
        {
            _parentController = parentController;
            _tableModel = new ContractorDetailsTableViewModel(contractor);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if ( IsCallOrTextCell(indexPath) )
                return GetCallOrTextContactCell(tableView, indexPath);
            else if ( IsEmailCell(indexPath))
                return GetEmailContactCell(tableView, indexPath);
            else if (IsSpecializationCell(indexPath))
                return GetSpecializationCell(tableView);
            else if (IsResumeCell(indexPath))
                return GetResumeCell(tableView);
            else if (IsContractsCell(indexPath))
                return GetContractsCell(tableView);

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

                vc.setContracts(_tableModel.Contracts);

                _parentController.ShowViewController(vc, _parentController);
            }
			if (IsResumeCell (indexPath)) 
			{
				ResumeViewController vc = (ResumeViewController) _parentController.Storyboard.InstantiateViewController("ResumeViewController");
		
				vc.Resume = _tableModel.ContractorResume;
				_parentController.ShowViewController(vc, _parentController);

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

        private int _resumeCellRow { get { return _specializationCellRow + 1; } }
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
        }
    }
}