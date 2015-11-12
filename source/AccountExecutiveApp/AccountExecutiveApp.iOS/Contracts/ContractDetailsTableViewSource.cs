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
				return 3;
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
					mainText = _contract.consultant.FullName;
					subtitleText = "Contractor";
				}
				if (indexPath.Item == 1) 
				{
					mainText = "Lucy Lu";
					subtitleText = "Client Contact";
				}
				if (indexPath.Item == 2) 
				{
					mainText = "Henry Ford";
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

