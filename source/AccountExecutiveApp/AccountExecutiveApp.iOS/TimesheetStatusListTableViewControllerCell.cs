
using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public class TimesheetStatusListTableViewControllerCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("TimesheetStatusListTableViewControllerCell");

		public TimesheetStatusListTableViewControllerCell () : base (UITableViewCellStyle.Value1, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			TextLabel.Text = "TextLabel";
		}
	}
}

