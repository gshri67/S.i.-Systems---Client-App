
using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class SearchSectionTotalResultsTableViewController : UITableViewController
	{
		public SearchSectionTotalResultsTableViewController (IntPtr handle) : base (handle)
		{
			//_viewModel = DependencyResolver.Current.Resolve<SearchViewModel>();
		}


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

