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
        private readonly ContractRateSupportViewModel _supportModel;
        private float _specializationCellHeight = -1;
        private int _numRateSections = 0;
        private int deletingSectionIndex = -1;
        private bool ShowHoursPerDay { get{ return _contractModel.AreRateTypesPerDay; } }

        public ContractCreationPayRatesTableViewSource(ContractCreationPayRatesTableViewController parentController, ContractCreationViewModel model, ContractRateSupportViewModel supportModel )
        {
            _parentController = parentController;
            _contractModel = model;
            _supportModel = supportModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = new UITableViewCell();

            if (IsIndexFromCell(indexPath, _rateTypeCellRow))
                return GetRateTypeCell(tableView, indexPath);
            else if ( ShowHoursPerDay && IsIndexFromCell(indexPath, _hoursPerDayCellRow))
                return GetHoursPerDayCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _rateDescriptionCellRow))
                return GetRateDescriptionCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _billRateCellRow))
                return GetBillRateCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _payRateCellRow))
                return GetPayRateCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _grossMarginCellRow))
                return GetGrossMarginCell(tableView, indexPath);
            else if (IsIndexFromCell(indexPath, _isPrimaryRateCellRow))
                return GetIsPrimaryRateCell(tableView, indexPath);

            return cell;
        }

        private int _rateTypeCellRow { get { return 0; } }
        private int _hoursPerDayCellRow { get { return _rateTypeCellRow + 1; } }
        private int _rateDescriptionCellRow 
        {
            get
            {
                if( !ShowHoursPerDay )
                    return _rateTypeCellRow + 1;
                return _hoursPerDayCellRow + 1;
            } 
        }
        private int _billRateCellRow { get { return _rateDescriptionCellRow + 1; } }
        private int _payRateCellRow { get { return _billRateCellRow + 1; } }
        private int _grossMarginCellRow { get { return _payRateCellRow + 1; } }
        private int _isPrimaryRateCellRow { get { return _grossMarginCellRow + 1; } }

        private UITableViewCell GetRateTypeCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);

            cell.OnValueChanged += delegate(string newValue)
            {
                _contractModel.SetRateTypeAtIndex(newValue, (int)indexPath.Section);

                EvaluateDynamicCells(tableView);
            };
            cell.UpdateCell("Rate Type", _supportModel.RateTypeOptions, _contractModel.RateTypeAtIndex((int)indexPath.Section));

            return cell;
        }

        private UITableViewCell GetHoursPerDayCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                //(EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
                new EditableNumberFieldCell(EditableNumberFieldCell.CellIdentifier);

            if (cell != null)
            {
                cell.UpdateCell("Hours", _contractModel.HoursPerDayAtIndex((int) indexPath.Section).ToString());
                cell.OnValueChanged +=
                    delegate(float newValue)
                    {
                        
                    };
                cell.OnValueFinalized +=
                    delegate(float newValue)
                    {
                        _contractModel.SetHoursPerDayAtIndex((int)newValue, (int)indexPath.Section);
                    };
                cell.UsingDollarSign = false;
                cell.UserInteractionEnabled = true;
            }

            return cell;
        }

        private UITableViewCell GetRateDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.SetRateDescriptionAtIndex(newValue, (int)indexPath.Section); };
            cell.UpdateCell("Rate Description", _supportModel.RateDescriptionOptions, _contractModel.RateDescriptionAtIndex((int)indexPath.Section));
            return cell;
        }

        private UITableViewCell GetBillRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
             //   (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
                new EditableNumberFieldCell(EditableNumberFieldCell.CellIdentifier);

            if (cell != null)
            {
                cell.UpdateCell("Bill Rate", _contractModel.BillRateAtIndex((int) indexPath.Section));
                cell.OnValueChanged += delegate(float newValue)
                {
                    _contractModel.SetBillRateAtIndex(newValue.ToString(), (int) indexPath.Section);
                };
                cell.OnValueFinalized += delegate(float newValue)
                {
                    //to update the gross margin we reload the table
                    tableView.ReloadData();
                };
                cell.UsingDollarSign = true;
                cell.UserInteractionEnabled = true;
            }
            return cell;
        }

        private UITableViewCell GetPayRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
                //(EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
                new EditableNumberFieldCell(EditableNumberFieldCell.CellIdentifier);

            if (cell != null)
            {
                cell.UpdateCell("Pay Rate", _contractModel.PayRateAtIndex((int) indexPath.Section));
                cell.OnValueChanged += delegate(float newValue)
                {
                    _contractModel.SetPayRateAtIndex(newValue.ToString(), (int) indexPath.Section);
                };
                cell.OnValueFinalized += delegate(float newValue)
                {
                    //to update the gross margin we reload the table
                    tableView.ReloadData();
                };

                cell.UsingDollarSign = true;
                cell.UserInteractionEnabled = true;
            }

            return cell;
        }

        private UITableViewCell GetGrossMarginCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditableNumberFieldCell cell =
               // (EditableNumberFieldCell)tableView.DequeueReusableCell(EditableNumberFieldCell.CellIdentifier, indexPath);
                           new EditableNumberFieldCell(EditableNumberFieldCell.CellIdentifier);

            if (cell != null)
            {
                cell.UpdateCell("GM", _contractModel.GrossMarginAtIndex((int) indexPath.Section));
                cell.UserInteractionEnabled = false;
                cell.UsingDollarSign = true;
            }
            return cell;
        }

        private UITableViewCell GetIsPrimaryRateCell(UITableView tableView, NSIndexPath indexPath)
        {
            EditablePickerCell cell =
                (EditablePickerCell)tableView.DequeueReusableCell(EditablePickerCell.CellIdentifier, indexPath);
            cell.UpdateCell("Primary Rate", _contractModel.IsPrimaryRateAtIndex((int)indexPath.Section));
            cell.OnValueChanged += delegate(string newValue)
            {
                if (newValue == "Yes")  _contractModel.SetPrimaryRateForIndex((int)indexPath.Section);
                tableView.ReloadData();
            };

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
            return _isPrimaryRateCellRow + 1;
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

        private void EvaluateDynamicCells(UITableView tableView)
        {
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