using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	partial class ContractDetailsViewController : UIViewController
	{
		public ConsultantContract _contract;
		public string subtitle;

		public ContractDetailsViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			tableView.RegisterClassForCellReuse ( typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell" );

			tableView.Source = new ContractDetailsTableViewSource (this, _contract );
			tableView.ReloadData ();

			UpdateSummaryView ();
		}

		public void UpdateSummaryView()
		{
			if (_contract == null)
				return;
			
			CompanyNameLabel.Text = _contract.CompanyName;
			PeriodLabel.Text = string.Format ( "{0} - {1}", _contract.StartDate.ToString("MMM dd YYY"),  _contract.EndDate.ToString("MM dd YYY") );
		}
	}
}
