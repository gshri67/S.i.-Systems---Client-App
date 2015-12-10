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
		UIKit.UILabel ClientAndStatusLabel { get; set; }

		[Outlet]
		UIKit.UILabel ContractTitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel GrossMarginLabel { get; set; }

		[Outlet]
		UIKit.UILabel MarginLabel { get; set; }

		[Outlet]
		UIKit.UILabel MarkupLabel { get; set; }

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
			if (BillRateLabel != null) {
				BillRateLabel.Dispose ();
				BillRateLabel = null;
			}

			if (ClientAndStatusLabel != null) {
				ClientAndStatusLabel.Dispose ();
				ClientAndStatusLabel = null;
			}

			if (ContractTitleLabel != null) {
				ContractTitleLabel.Dispose ();
				ContractTitleLabel = null;
			}

			if (GrossMarginLabel != null) {
				GrossMarginLabel.Dispose ();
				GrossMarginLabel = null;
			}

			if (MarkupLabel != null) {
				MarkupLabel.Dispose ();
				MarkupLabel = null;
			}

			if (PayRateLabel != null) {
				PayRateLabel.Dispose ();
				PayRateLabel = null;
			}

			if (PeriodLabel != null) {
				PeriodLabel.Dispose ();
				PeriodLabel = null;
			}

			if (summaryView != null) {
				summaryView.Dispose ();
				summaryView = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (MarginLabel != null) {
				MarginLabel.Dispose ();
				MarginLabel = null;
			}
		}
	}
}
