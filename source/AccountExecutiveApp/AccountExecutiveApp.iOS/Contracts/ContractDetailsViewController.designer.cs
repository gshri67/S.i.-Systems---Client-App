// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AccountExecutiveApp.iOS
{
	[Register ("ContractDetailsViewController")]
	partial class ContractDetailsViewController
	{
		[Outlet]
		UIKit.UILabel BillRateLabel { get; set; }

		[Outlet]
		UIKit.UILabel CompanyNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel GrossMarginLabel { get; set; }

		[Outlet]
		UIKit.UILabel PayRateLabel { get; set; }

		[Outlet]
		UIKit.UILabel PeriodLabel { get; set; }

		[Outlet]
		UIKit.UIView summaryView { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (summaryView != null) {
				summaryView.Dispose ();
				summaryView = null;
			}

			if (PeriodLabel != null) {
				PeriodLabel.Dispose ();
				PeriodLabel = null;
			}

			if (BillRateLabel != null) {
				BillRateLabel.Dispose ();
				BillRateLabel = null;
			}

			if (GrossMarginLabel != null) {
				GrossMarginLabel.Dispose ();
				GrossMarginLabel = null;
			}

			if (PayRateLabel != null) {
				PayRateLabel.Dispose ();
				PayRateLabel = null;
			}

			if (CompanyNameLabel != null) {
				CompanyNameLabel.Dispose ();
				CompanyNameLabel = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}
