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
    public class ContractCreationPayRatesTableViewSource : UITableViewSource
    {
        private readonly ContractCreationPayRatesTableViewController _parentController;
        private readonly ContractCreationViewModel _contractModel;
        private float _specializationCellHeight = -1;
        private int _numRateSections = 0;

        public ContractCreationPayRatesTableViewSource(ContractCreationPayRatesTableViewController parentController, ContractCreationViewModel model)
        {
            _parentController = parentController;
            _contractModel = model;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = new UITableViewCell();
           
            if (IsRateTypeCell(indexPath))
                return GetRateTypeCell(tableView, indexPath);
            else if (IsRateDescriptionCell(indexPath))
                return GetRateDescriptionCell(tableView, indexPath);
            else if (IsBillRateCell(indexPath))
                return GetBillRateCell(tableView, indexPath);
            else if (IsIsPrimaryRateCell(indexPath))
                return GetIsPrimaryRateCell(tableView, indexPath);

            return cell;
        }

        private int _rateTypeCellRow { get { return 0; } }
        private bool IsRateTypeCell(NSIndexPath indexPath) { return (int)indexPath.Item == _rateTypeCellRow; }

        private UITableViewCell GetRateTypeCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.UpdateCell("Rate Type", _contractModel.RateTypeOptions, _contractModel.RateTypeSelectedIndexAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateTypeAtIndex(newValue, (int)indexPath.Section); };

            return cell;
        }

        private int _rateDescriptionCellRow { get { return _rateTypeCellRow + 1; } }
        private bool IsRateDescriptionCell(NSIndexPath indexPath) { return (int)indexPath.Item == _rateDescriptionCellRow; }

        private UITableViewCell GetRateDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Rate Description", _contractModel.RateDescriptionOptions, _contractModel.RateDescriptionSelectedIndexAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateDescriptionAtIndex( newValue, (int)indexPath.Section); };

            return cell;
        }

        private int _billRateCellRow { get { return _rateDescriptionCellRow + 1; } }
        private bool IsBillRateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _billRateCellRow; }

        private UITableViewCell GetBillRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Bill Rate", _contractModel.BillRateAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(float newValue)
            {
                _contractModel.SetBillRateAtIndex(newValue.ToString(), (int) indexPath.Section);
            };

            return cell;
        }

        private int _isPrimaryRateCellRow { get { return _billRateCellRow + 1; } }
        private bool IsIsPrimaryRateCell(NSIndexPath indexPath) { return (int)indexPath.Item == _isPrimaryRateCellRow; }

        private UITableViewCell GetIsPrimaryRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Primary Rate", _contractModel.IsPrimaryRateAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(bool newValue) { _contractModel.SetPrimaryRateForIndex((int)indexPath.Section); };

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 4;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _numRateSections;
        }
        
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 35;
        }
        /*
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 15;
        }
        */
        public void AddRatesSection(UITableView tableView)
        {
            _numRateSections ++;
            _contractModel.AddRate();

            tableView.ReloadData();
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return string.Format("Pay Rate {0}", (int) section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UILabel sectionLabel = new UILabel();
            sectionLabel.Text = string.Format("Pay Rate {0}", (int) section);
            return sectionLabel;
        }
    }
}