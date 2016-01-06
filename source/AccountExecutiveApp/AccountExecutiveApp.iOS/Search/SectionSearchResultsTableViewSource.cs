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
    public class SectionSearchResultsTableViewSource : UITableViewSource
    {
        private readonly SearchSectionTotalResultsTableViewController _parentController;
        private readonly ContractorDetailsTableViewModel _parentModel;
        private SectionSearchResultsTableViewModel _tableModel;

        public SectionSearchResultsTableViewSource(SearchSectionTotalResultsTableViewController parentController, IEnumerable<UserContact> contacts )
        {
            _parentController = parentController;
            _tableModel = new SectionSearchResultsTableViewModel( contacts );
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (RightDetailCell) tableView.DequeueReusableCell(RightDetailCell.CellIdentifier, indexPath);

            string mainText = string.Empty, rightDetailText = string.Empty;

            mainText = _tableModel.ContactNameByRowNumber((int)indexPath.Item);

            cell.UpdateCell(mainText: mainText, rightDetailText: rightDetailText);

            return cell;
        }  

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (_tableModel != null)
                return _tableModel.NumberOfContacts;

            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            /*
                if (IsClientContactCell(indexPath))
                {
                    var vc =
                        (ClientContactDetailsViewController)
                            _parentController.Storyboard.InstantiateViewController("ClientContactDetailsViewController");
                    vc.SetContactId(_tableModel.GetClientContactIdForIndex((int) indexPath.Item),
                        _tableModel.GetClientContactTypeForIndex((int) indexPath.Item));
                    _parentController.ShowViewController(vc, _parentController);
                }
                else if (IsContractorCell(indexPath))
                {
                    var vc =
                        (ContractorDetailsTableViewController)
                            _parentController.Storyboard.InstantiateViewController(
                                "ContractorDetailsTableViewController");
                    vc.setContractorId(_tableModel.GetContractorIdForIndex((int) indexPath.Item));
                    _parentController.ShowViewController(vc, _parentController);
                }*/
        }
  
    }
}