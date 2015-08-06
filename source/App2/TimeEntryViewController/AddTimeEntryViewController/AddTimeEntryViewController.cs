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
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				//DismissViewController(true, null);
				NavigationController.PopViewController(true);
			};
		}
	}
}
