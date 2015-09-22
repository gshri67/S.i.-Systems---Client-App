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
			if (_remittances == null || _remittances.Count () == 0)
				return 0;

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
			return 0;
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
					//period: string.Format( "{0:MMM} {0:d}-{1:d}", remittance.StartDate, remittance.EndDate )
					period: remittance.StartDate.ToString("MMM ") + remittance.StartDate.ToString("d ").Trim(' ') + "-" + remittance.EndDate.ToString("d ").Trim(' ')
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

		public override UIView GetViewForFooter (UITableView tableView, nint section)
		{
			UILabel footerView = new UILabel ();
			footerView.Text = "Please use the Desktop portal to view eRemittances older than 6 months";
			footerView.Lines = 0;
			footerView.BackgroundColor = StyleGuideConstants.LighterGrayUiColor;
			footerView.TextAlignment = UITextAlignment.Center;

			return footerView;
		}

		public override nfloat GetHeightForFooter (UITableView tableView, nint section)
		{
			return 150;
		}
		/*
		public Timesheet GetItem(NSIndexPath indexPath)
		{
			return _payPeriods.ElementAt(indexPath.Section).Timesheets.ElementAt(indexPath.Row);
		}*/
	}
}