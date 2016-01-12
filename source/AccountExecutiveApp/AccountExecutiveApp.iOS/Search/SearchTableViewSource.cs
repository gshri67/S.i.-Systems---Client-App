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
    public class SearchTableViewSource : UITableViewSource
    {
        private readonly SearchTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;
        private SearchTableViewModel _tableModel;
        private Dictionary<string, int> categoryIndex;

        public SearchTableViewSource(SearchTableViewController parentController, IEnumerable<UserContact> clientContacts,
            IEnumerable<UserContact> contractors)
        {
            _parentController = parentController;
            _tableModel = new SearchTableViewModel(clientContacts, contractors);
            categoryIndex = new Dictionary<string, int>();

            SetupCategoryIndexDictionary();
        }

        private void StyleCategoryCell(RightDetailCell cell)
        {
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            StyleLabelForCategoryCell(cell.MainTextLabel);
            StyleLabelForCategoryCell(cell.RightDetailTextLabel);

            cell.BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 0.0f);
        }

        private void StyleLabelForCategoryCell(UILabel label)
        {
            label.Font = UIFont.FromName("Helvetica", 12f);
            label.TextColor = UIColor.DarkTextColor;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (RightDetailCell) tableView.DequeueReusableCell(RightDetailCell.CellIdentifier, indexPath);

            string mainText = string.Empty, rightDetailText = string.Empty;

            if (IsCategoryCell(indexPath))
            {
                if (IsClientContactCell(indexPath))
                {
                    mainText = @"CLIENT CONTACTS";
                    rightDetailText = _tableModel.NumberOfTotalFilteredClientContacts.ToString();
                }
                else if (IsContractorCell(indexPath))
                {
                    mainText = @"CONTRACTORS";

                    rightDetailText = _tableModel.NumberOfTotalFilteredContractors.ToString();
                }

                StyleCategoryCell(cell);
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.None;

                if (IsClientContactCell(indexPath))
                {
                    mainText = _tableModel.ClientContactNameByRowNumber((int) indexPath.Item - 1);
                    rightDetailText = _tableModel.ClientCompanyNameByRowNumber((int) indexPath.Item - 1);


                }
                else if (IsContractorCell(indexPath))
                    mainText =
                        _tableModel.ContractorNameByRowNumber((int) indexPath.Item - _firstContractorCellIndex - 1);
            }

            cell.UpdateCell(mainText: mainText, rightDetailText: rightDetailText);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (_tableModel != null)
            {
                int rows = _tableModel.NumberOfClientContacts + _tableModel.NumberOfContractors;

                if (_tableModel.NumberOfClientContacts > 0)
                    rows ++;
                if (_tableModel.NumberOfContractors > 0)
                    rows++;

                return rows;
            }
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsCategoryCell(indexPath))
            {
                if (IsClientContactCell(indexPath))
                {
                    var vc =
                        (SearchSectionTotalResultsTableViewController)
                            _parentController.Storyboard.InstantiateViewController(
                                "SearchSectionTotalResultsTableViewController");
                    vc.SetData(_tableModel.GetFilteredResultsForClientContacts(), true);
                    _parentController.ShowViewController(vc, _parentController);
                }
                else if (IsContractorCell(indexPath))
                {
                    var vc =
                        (SearchSectionTotalResultsTableViewController)
                            _parentController.Storyboard.InstantiateViewController(
                                "SearchSectionTotalResultsTableViewController");
                    vc.SetData(_tableModel.GetFilteredResultsForContractors(), false);
                    _parentController.ShowViewController(vc, _parentController);
                }
            }
            else
            {
                int index;

                if (IsClientContactCell(indexPath))
                {
                    index = (int) indexPath.Item - _firstClientContactCellIndex - 1;

                    var vc =
                        (ClientContactDetailsViewController)
                            _parentController.Storyboard.InstantiateViewController("ClientContactDetailsViewController");
                    vc.showSearchIcon = false;
                    vc.SetContactId(_tableModel.GetClientContactIdForIndex(index),
                        _tableModel.GetClientContactTypeForIndex(index));
                    _parentController.ShowViewController(vc, _parentController);
                }
                else if (IsContractorCell(indexPath))
                {
                    index = (int) indexPath.Item - _firstContractorCellIndex - 1;

                    var vc =
                        (ContractorDetailsTableViewController)
                            _parentController.Storyboard.InstantiateViewController(
                                "ContractorDetailsTableViewController");
                    vc.ShowSearchIcon = false;
                    vc.setContractorId(_tableModel.GetContractorIdForIndex(index));
                    _parentController.ShowViewController(vc, _parentController);
                }
            }

            UITableViewCell selectedCell = tableView.CellAt(indexPath);
            selectedCell.SetSelected(false, false);
        }

        private bool IsCategoryCell(NSIndexPath indexPath)
        {
            if (categoryIndex.Values.Where(x => x == (int) indexPath.Item).Any())
                return true;
            return false;
        }

        private bool IsClientContactCell(NSIndexPath indexPath)
        {
            if (_tableModel.NumberOfClientContacts > 0 && indexPath.Item >= _firstClientContactCellIndex &&
                indexPath.Item < _firstClientContactCellIndex + _tableModel.NumberOfClientContacts + 1)
                return true;
            return false;
        }

        private bool IsContractorCell(NSIndexPath indexPath)
        {
            if (_tableModel.NumberOfContractors > 0 && indexPath.Item >= _firstContractorCellIndex &&
                indexPath.Item < _firstContractorCellIndex + _tableModel.NumberOfContractors + 1)
                return true;
            return false;
        }

        private int _firstContractorCellIndex
        {
            get
            {
                if (categoryIndex.ContainsKey("Contractors")) return categoryIndex["Contractors"];
                else return -1;
            }
        }

        private int _firstClientContactCellIndex
        {
            get
            {
                if (categoryIndex.ContainsKey("Client Contacts")) return categoryIndex["Client Contacts"];
                else return -1;
            }
        }

        public void SetupCategoryIndexDictionary()
        {
            categoryIndex.Clear();

            bool existsClientContacts = _tableModel.NumberOfClientContacts > 0;
            bool existsContractors = _tableModel.NumberOfContractors > 0;

            if (existsClientContacts)
            {
                categoryIndex["Client Contacts"] = 0;

                if (existsContractors)
                    categoryIndex["Contractors"] = _tableModel.NumberOfClientContacts + 1;
            }
            else
                categoryIndex["Contractors"] = 0;
        }

        public void ReloadWithFilteredContacts(IEnumerable<UserContact> filteredClientContacts,
            IEnumerable<UserContact> filteredContractors)
        {
            _tableModel.ReloadWithFilteredContacts(filteredClientContacts, filteredContractors);
            SetupCategoryIndexDictionary();
        }
    }
}