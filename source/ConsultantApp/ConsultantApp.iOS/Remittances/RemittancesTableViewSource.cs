using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS
{
	internal class RemittancesTableViewSource : UITableViewSource
	{
		private const string CellIdentifier = "remittanceCell";
		private readonly UIViewController _parentController;

		private readonly List<Remittance> _remittances;

		public RemittancesTableViewSource(UIViewController parentController, IEnumerable<Remittance> remittances) 
		{
			this._parentController = parentController;

			if (remittances != null) 
			{
				_remittances = remittances.OrderByDescending (pp => pp.EndDate).ToList ();
			}
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}
		/*
		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return _payPeriods[(int)section] != null
				? _payPeriods[(int)section].TimePeriod
					: "Unknown Pay Period";
		}*/

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if( _remittances != null )
				return _remittances.Count();
			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			var cell = tableView.DequeueReusableCell(CellIdentifier) as RemittanceCell ??
				new RemittanceCell(CellIdentifier);
			
			if (_remittances != null) 
			{
				Remittance remittance = _remittances.ElementAt ((int)indexPath.Item);
				cell.UpdateCell (
					depositDate: remittance.DepositDate.ToString("MMM ") + remittance.DepositDate.ToString("dd, yyyy").Trim('0'),
					documentNumber: remittance.DocumentNumber,
					amount: remittance.Amount,
					period: remittance.StartDate.ToString("MMM ") + remittance.StartDate.ToString("dd").Trim('0') + "-" + remittance.EndDate.ToString("dd").Trim('0')
					//remittance.StartDate.ToString("MM/").Trim('0') + remittance.StartDate.ToString("dd").Trim('0') + "-" + remittance.EndDate.ToString("MM/").Trim('0') + remittance.EndDate.ToString("dd").Trim('0')
				);
			}


			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			/*
			_parentController.PerformSegue(ActiveTimesheetViewController.TimesheetSelectedSegue, indexPath);

			//normal iOS behaviour is to remove the selection
			tableView.DeselectRow(indexPath, true);*/
		}
		/*
		public Timesheet GetItem(NSIndexPath indexPath)
		{
			return _payPeriods.ElementAt(indexPath.Section).Timesheets.ElementAt(indexPath.Row);
		}*/
	}
}