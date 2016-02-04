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

        public ContractCreationPayRatesTableViewSource (ContractCreationPayRatesTableViewController parentController, ContractCreationViewModel model)
		{
			_parentController = parentController;
		    _contractModel = model;
		}

	    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
	    {
            ContractCreationRateCell cell = (ContractCreationRateCell)tableView.DequeueReusableCell(ContractCreationRateCell.CellIdentifier, indexPath);

	        return cell;
	        /*
	        int row = (int) indexPath.Item;

            if (IsJobTitleCell(indexPath))
                return GetJobTitleCell(tableView, indexPath);
            
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);

            return cell;*/
	    }

	    private int _jobTitleCellRow { get { return 0; } }
        private bool IsJobTitleCell(NSIndexPath indexPath) { return (int)indexPath.Item == _jobTitleCellRow; }

	    private UITableViewCell GetJobTitleCell(UITableView tableView, NSIndexPath indexPath)
	    {
            EditableTextFieldCell cell = (EditableTextFieldCell)tableView.DequeueReusableCell(EditableTextFieldCell.CellIdentifier, indexPath);
            cell.UpdateCell("Job Title", _contractModel.JobTitle);
            cell.OnValueChanged += delegate(string newValue) { _contractModel.JobTitle = newValue; };

	        return cell;
	    }
        
		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 2;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        return 90;
	    }
	}
}