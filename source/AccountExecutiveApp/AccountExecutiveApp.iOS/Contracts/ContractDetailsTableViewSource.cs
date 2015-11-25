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
				string mainText = "Bob Smith", subtitleText = "";

				if (indexPath.Item == 0) 
				{
					mainText = _contract.Contractor.FullName;
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
		/*
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			ContractDetailsViewController vc = (ContractDetailsViewController)_parentController.Storyboard.InstantiateViewController ("ContractDetailsViewController");

			_parentController.ShowViewController ( vc, _parentController );
		}*/
	}
}

