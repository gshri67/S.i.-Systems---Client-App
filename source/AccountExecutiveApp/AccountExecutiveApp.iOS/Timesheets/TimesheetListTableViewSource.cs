using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.Core;

namespace AccountExecutiveApp.iOS
{
	public class TimesheetListTableViewSource : UITableViewSource
	{
		private readonly TimesheetListTableViewController _parentController;
		private readonly TimesheetListTableViewModel _listViewModel;

		public TimesheetListTableViewSource(TimesheetListTableViewController parentViewController, IEnumerable<TimesheetDetails> details )
		{
			_listViewModel = new TimesheetListTableViewModel(details);

			_parentController = parentViewController;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
            int numSections =_listViewModel.NumberOfGroups();
		    
            if (numSections == 0)
		        numSections = 1;
		    
            return numSections;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return _listViewModel.NumberOfTimesheetsInSection((int)section);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(SubtitleWithRightDetailCell.CellIdentifier) as SubtitleWithRightDetailCell;

            if( cell != null )
		        cell.UpdateCell(
                    mainText: _listViewModel.ContractorFullNameBySectionAndRow((int)indexPath.Section, (int)indexPath.Item),
		            subtitleText: _listViewModel.CompanyNameBySectionAndRow((int)indexPath.Section, (int)indexPath.Item),
                    rightDetailText: _listViewModel.FormattedPeriodBySectionAndRow((int)indexPath.Section, (int)indexPath.Item)
                );

            

			return cell;
		}

		
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            var viewController = (TimesheetContactsTableViewController)_parentController.Storyboard.InstantiateViewController("TimesheetContactsTableViewController");
            viewController.LoadTimesheetContact(_listViewModel.TimesheetIdBySectionAndRow((int)indexPath.Section, (int)indexPath.Item));
			_parentController.ShowViewController(viewController, _parentController);
		}

	    public override string TitleForHeader(UITableView tableView, nint section)
	    {
	        return _listViewModel.YearBySection((int) section);
	    }
	}
}

