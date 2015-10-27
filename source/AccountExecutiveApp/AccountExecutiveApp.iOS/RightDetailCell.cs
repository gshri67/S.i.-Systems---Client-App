using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class RightDetailCell : UITableViewCell
	{
		public RightDetailCell (IntPtr handle) : base (handle)
		{
		}

		[Export("initWithStyle:reuseIdentifier:")]
		public RightDetailCell(UITableViewCellStyle style, string cellIdentifier)
			: base(UITableViewCellStyle.Value1, cellIdentifier)
		{

		}
	}
}