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
        private int deletingSectionIndex = -1;

        public ContractCreationPayRatesTableViewSource(ContractCreationPayRatesTableViewController parentController, ContractCreationViewModel model)
        {
            _parentController = parentController;
            _contractModel = model;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = new UITableViewCell();

            if (IsIndexFromCell(indexPath, _rateTypeCellRow))
                return GetRateTypeCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _rateDescriptionCellRow))
                return GetRateDescriptionCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _billRateCellRow))
                return GetBillRateCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _isPrimaryRateCellRow))
                return GetIsPrimaryRateCell(tableView, indexPath);

            return cell;
        }

        private int _rateTypeCellRow { get { return 0; } }

        private UITableViewCell GetRateTypeCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.UpdateCell("Rate Type", _contractModel.RateTypeOptions, _contractModel.RateTypeSelectedIndexAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateTypeAtIndex(newValue, (int)indexPath.Section); };

            return cell;
        }

        private int _rateDescriptionCellRow { get { return _rateTypeCellRow + 1; } }

        private UITableViewCell GetRateDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Rate Description", _contractModel.RateDescriptionOptions, _contractModel.RateDescriptionSelectedIndexAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateDescriptionAtIndex( newValue, (int)indexPath.Section); };

            return cell;
        }

        private int _billRateCellRow { get { return _rateDescriptionCellRow + 1; } }

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

        private UITableViewCell GetIsPrimaryRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableBooleanCell cell =
                (EditableBooleanCell)tableView.DequeueReusableCell(EditableBooleanCell.CellIdentifier, indexPath);
            cell.UpdateCell("Primary Rate", _contractModel.IsPrimaryRateAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(bool newValue) { _contractModel.SetPrimaryRateForIndex((int)indexPath.Section); };

            return cell;
        }

        private bool IsIndexFromCell(NSIndexPath indexPath, int cellRow)
        {
            if ((int)indexPath.Item == cellRow)
                return true;
            return false;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
           // if (deletingSectionIndex == section )
             //   return 0;

            return 4;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _contractModel.NumRates;
        }
        
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 35;
        }
        
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 50;
        }
        
        public void AddRatesSection(UITableView tableView)
        {
            _contractModel.AddRate();

            tableView.ReloadData();
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return string.Format("Pay Rate {0}", (int) section);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            DeletableSection deletableSection = new DeletableSection();

            deletableSection.SectionLabel.Text = string.Format("RATE {0}", (int)section + 1); 

            deletableSection.DeleteButton.TouchUpInside += delegate
            {
                deletingSectionIndex = (int)section;
                /*
                tableView.DeleteRows(new NSIndexPath[]
                {
                    NSIndexPath.FromItemSection(0, section),
                    NSIndexPath.FromItemSection(1, section),
                    NSIndexPath.FromItemSection(2, section),
                    NSIndexPath.FromItemSection(3, section)
                }, UITableViewRowAnimation.Automatic );
                */

                _contractModel.DeleteRate((int)section);
                tableView.DeleteSections( NSIndexSet.FromIndex(section), UITableViewRowAnimation.Automatic );

                deletingSectionIndex = -1;
               
                tableView.ReloadData();

                //var vc = (ContractCreationSendingTableViewController)Storyboard.InstantiateViewController("ContractCreationSendingTableViewController");
                //vc.ViewModel = ViewModel;
                //ShowViewController(vc, this);
            };

            return deletableSection;
        }
    }
}