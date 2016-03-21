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

	    private bool RemittancesExist()
	    {
	        return _remittances != null && _remittances.Any();
	    }

        private int NumberOfRemittances()
        {
            return RemittancesExist()
                ? _remittances.Count()
                : 0;
        }
        
		public override nint NumberOfSections(UITableView tableView)
		{
			return RemittancesExist()
                ? 1
                : 0;
		}

	    public override nint RowsInSection(UITableView tableview, nint section)
		{
		    return NumberOfRemittances();
		}

	    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			// if there are no cells to reuse, create a new one
			var cell = tableView.DequeueReusableCell(CellIdentifier) as RemittanceCell ??
				new RemittanceCell(CellIdentifier);
			
			PopulateCell(indexPath, cell);

			return cell;
		}

	    private void PopulateCell(NSIndexPath indexPath, RemittanceCell cell)
	    {
	        if (_remittances == null) return;

	        var remittance = _remittances.ElementAt((int) indexPath.Item);
	        cell.UpdateCell(
	            depositDate: remittance.DepositDate.ToString("MMM ") + remittance.DepositDate.ToString("dd, yyyy").Trim('0'),
	            documentNumber: remittance.DocumentNumber,
	            amount: remittance.Amount,
	            period: string.Empty
	            );
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var storyboard = _parentController.Storyboard;
		    var vc = (RemittanceSummaryViewController)storyboard.InstantiateViewController("RemittanceSummaryViewController");
    
            _parentController.ShowViewController(vc, _parentController);

            if (_remittances != null && _remittances.Count > (int)indexPath.Item)
                vc.SetRemittance(_remittances[(int)indexPath.Item]);
		}

		public override UIView GetViewForFooter(UITableView tableView, nint section)
		{
		    var footerView = new UILabel
		    {
		        Text = "Please use the Desktop portal to view eRemittances older than 6 months",
		        Lines = 0,
		        BackgroundColor = UIColor.Clear,
		        TextAlignment = UITextAlignment.Center
		    };

		    return footerView;
		}

		public override nfloat GetHeightForFooter (UITableView tableView, nint section)
		{
			return 150;
		}
	}
}