// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	[Register ("TimeSheetEntryViewController")]
	partial class TimeSheetEntryViewController
	{
		[Outlet]
		UIKit.UIButton addButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addButton != null) {
				addButton.Dispose ();
				addButton = null;
			}
		}
	}
}
