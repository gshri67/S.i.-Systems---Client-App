using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class AddTimeEntryViewController : UIViewController
	{
		public AddTimeEntryViewController (IntPtr handle) : base (handle)
		{
			addButton.TouchUpInside += delegate {
				DismissViewController(true, null);
			};
		}
	}
}
