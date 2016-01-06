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

		public SearchTableViewSource(SearchTableViewController parentController, IEnumerable<UserContact> clientContacts, IEnumerable<Contractor> contractors )
		{
			_parentController = parentController;
			_tableModel = new SearchTableViewModel( clientContacts, contractors );
			categoryIndex = new Dictionary<string, int> ();

			SetupCategoryIndexDictionary();
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			/*
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
			*/

            var cell = (RightDetailCell)tableView.DequeueReusableCell(RightDetailCell.CellIdentifier, indexPath);

		    string mainText = string.Empty, rightDetailText = string.Empty;

		    if (IsCategoryCell(indexPath))
		    {
		        if (IsClientContactCell(indexPath))
		        {
		            mainText = @"Client Contacts";
		            rightDetailText = _tableModel.NumberOfTotalFilteredContractors.ToString();
		        }
		        else if (IsContractorCell(indexPath))
		        {
		            mainText = @"Contractors";

		            rightDetailText = _tableModel.NumberOfTotalFilteredClientContacts.ToString();
		        }
		    }
		    else if (IsClientContactCell(indexPath))
		    {
		        mainText = _tableModel.ClientContactNameByRowNumber((int) indexPath.Item - 1);
		        rightDetailText = _tableModel.ClientCompanyNameByRowNumber((int) indexPath.Item - 1);
		    }
		    else if (IsContractorCell(indexPath))
		        mainText = _tableModel.ContractorNameByRowNumber((int) indexPath.Item - _firstIsContractorCellIndex - 1);

		    cell.UpdateCell( mainText: mainText, rightDetailText: rightDetailText );

		    return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
		    if (_tableModel != null)
		        return _tableModel.NumberOfClientContacts + _tableModel.NumberOfContractors + 2;

            return 0;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		/*
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
		}*/

		private bool IsCategoryCell(NSIndexPath indexPath)
		{
			if ( categoryIndex.Values.Where( x => x == (int)indexPath.Item ).Any() )
				return true;
			return false;
		}
        private bool IsClientContactCell(NSIndexPath indexPath)
        {
            if ( indexPath.Item >= _firstIsClientContactCellIndex && indexPath.Item < _firstIsClientContactCellIndex + _tableModel.NumberOfClientContacts+1)
                return true;
            return false;
        }
        private bool IsContractorCell(NSIndexPath indexPath)
        {
            if (indexPath.Item >= _firstIsContractorCellIndex && indexPath.Item < _firstIsContractorCellIndex + _tableModel.NumberOfContractors + 1)
                return true;
            return false;
        }

        private int _firstIsContractorCellIndex { get { return categoryIndex["Contractors"]; } }
        private int _firstIsClientContactCellIndex { get { return categoryIndex["Client Contacts"]; } }

	    public void ApplyFilterWithText(string text)
	    {
	        _tableModel.ApplyFilterWithText(text);

            SetupCategoryIndexDictionary();
	    }

	    public void SetupCategoryIndexDictionary()
	    {
            categoryIndex.Clear();
            categoryIndex["Client Contacts"] = 0;
            categoryIndex["Contractors"] = _tableModel.NumberOfClientContacts + 1;
	    }

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (IsCategoryCell(indexPath))
			{
				if (IsClientContactCell(indexPath))
				{
					var vc =
						(SearchSectionTotalResultsTableViewController)
						_parentController.Storyboard.InstantiateViewController("SearchSectionTotalResultsTableViewController");

					UINavigationController navVC = new UINavigationController (vc);

					_parentController.ShowViewController (vc, _parentController);
					//_parentController.PresentViewController(vc, true, null);
					//_parentController.NavigationController.PushViewController( vc, true );
				}
				else if (IsContractorCell(indexPath))
				{
					var vc =
						(SearchSectionTotalResultsTableViewController)
						_parentController.Storyboard.InstantiateViewController("SearchSectionTotalResultsTableViewController");

					_parentController.ShowViewController(vc, _parentController);
				}
			}

		}
	}
}