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
	[Register ("ClientContactDetailsViewController")]
	partial class ClientContactDetailsViewController
	{
		[Outlet]
		UIKit.UILabel AddressLabel { get; set; }

		[Outlet]
		UIKit.UILabel CompanyAddressLabel { get; set; }

		[Outlet]
		UIKit.UILabel CompanyNameLabel { get; set; }

		[Outlet]
		UIKit.UIView DetailsContainerView { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddressLabel != null) {
				AddressLabel.Dispose ();
				AddressLabel = null;
			}

			if (CompanyAddressLabel != null) {
				CompanyAddressLabel.Dispose ();
				CompanyAddressLabel = null;
			}

			if (CompanyNameLabel != null) {
				CompanyNameLabel.Dispose ();
				CompanyNameLabel = null;
			}

			if (DetailsContainerView != null) {
				DetailsContainerView.Dispose ();
				DetailsContainerView = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
