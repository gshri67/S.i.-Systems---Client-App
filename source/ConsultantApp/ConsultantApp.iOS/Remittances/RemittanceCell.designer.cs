// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ConsultantApp.iOS
{
	partial class RemittanceCell
	{
		[Outlet]
		UIKit.UILabel amountLabel { get; set; }

		[Outlet]
		UIKit.UILabel depositDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel periodLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (depositDateLabel != null) {
				depositDateLabel.Dispose ();
				depositDateLabel = null;
			}

			if (amountLabel != null) {
				amountLabel.Dispose ();
				amountLabel = null;
			}

			if (periodLabel != null) {
				periodLabel.Dispose ();
				periodLabel = null;
			}
		}
	}
}
