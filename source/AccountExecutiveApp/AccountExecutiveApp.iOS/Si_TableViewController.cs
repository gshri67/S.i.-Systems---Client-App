
using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class Si_TableViewController : UITableViewController
	{
		public bool ShowSearchIcon = true;

		public Si_TableViewController(IntPtr handle)
			: base(handle) { }

		public Si_TableViewController(){}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

