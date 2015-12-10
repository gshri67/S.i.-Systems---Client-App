using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class ContractDetailsTableViewSource : UITableViewSource
	{
		private readonly UIViewController _parentController;

		private ConsultantContract _contract;

		public ContractDetailsTableViewSource(UIViewController parentVC, ConsultantContract contract)
		{
			_parentController = parentVC;
			_contract = contract;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (_contract != null)
				return 4;
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			string CellIdentifier = "SubtitleWithRightDetailCell";
			var cell = tableView.DequeueReusableCell (CellIdentifier) as SubtitleWithRightDetailCell;

			if (_contract != null)
			{
				string mainText = "", subtitleText = "";

				if (indexPath.Item == 0) 
				{
                    mainText = _contract.Contractor.ContactInformation.FullName;
					subtitleText = "Contractor";
				}
				else if (indexPath.Item == 1) 
				{
					mainText = _contract.DirectReport.FullName;
					subtitleText = "Direct Report";					
				}
				else if (indexPath.Item == 2) 
				{
					mainText = _contract.ClientContact.FullName;
					subtitleText = "Client Contact";
				}
				else if (indexPath.Item == 3) {
					mainText = _contract.BillingContact.FullName;
					subtitleText = "Billing Contact";
				} 

				cell.UpdateCell 
				(
					mainText: mainText,
					subtitleText:subtitleText,
					rightDetailText:""
				);
			}

			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            if ((int) indexPath.Item == 0)
		    {
		        var vc =
		            (ContractorDetailsTableViewController)
		                _parentController.Storyboard.InstantiateViewController("ContractorDetailsTableViewController");
                vc.setContractorId(GetContactIdForIndex(indexPath));
		        _parentController.ShowViewController(vc, _parentController);
		    }
		    else
		    {
                var contactTypeSelected = GetContactTypeForIndex(indexPath);
                var vc = (ClientContactDetailsViewController) _parentController.Storyboard.InstantiateViewController("ClientContactDetailsViewController");
                vc.SetContactId( GetContactIdForIndex(indexPath), contactTypeSelected);
                _parentController.ShowViewController(vc, _parentController);
		    }
		}

	    private int GetContactIdForIndex(NSIndexPath indexPath)
	    {
            switch (indexPath.Item)
            {
                case 1:
                    return _contract.DirectReport.Id;
                case 2:
                    return _contract.ClientContact.Id;
                case 3:
                    return _contract.BillingContact.Id;
                default:
                    return _contract.Contractor.ContactInformation.Id;
            }
	    }

	    private static UserContactType GetContactTypeForIndex(NSIndexPath indexPath)
	    {
	        switch (indexPath.Item)
	        {
                case 1:
	                return UserContactType.DirectReport;
                case 2:
                    return UserContactType.ClientContact;
                case 3:
                    return UserContactType.BillingContact;
                default: 
                    return UserContactType.Contractor;
	        }
	    }
	}
}

