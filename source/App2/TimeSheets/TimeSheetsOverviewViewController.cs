
using System;

using Foundation;
using UIKit;

namespace App2
{
	public partial class TimeSheetsOverviewViewController : UIViewController
	{
		public TimeSheetsOverviewViewController () : base ("TimeSheetsOverviewViewController", null)
		{
            int i;
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
            //tableview.Delegate = new UITableViewDelegate();
            //tableview.DataSource = new UITableViewDataSource();
            //tableview.RegisterClassForCellReuse(typeof(UITableViewCell), "cell" );
            
            tableview.Source = new TimeSheetsOverviewTableViewSource( this );
		}

       
	}
}

