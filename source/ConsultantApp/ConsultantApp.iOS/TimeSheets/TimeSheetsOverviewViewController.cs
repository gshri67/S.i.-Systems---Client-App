using System;
using UIKit;

namespace ConsultantApp.iOS.TimeSheets
{
	partial class TimeSheetsOverviewViewController : UIViewController
	{


		public TimeSheetsOverviewViewController (IntPtr handle) : base (handle)
		{

		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//tableview = new UITableView(View.Bounds);
            
            //EdgesForExtendedLayout = UIRectEdge.None;
            //ExtendedLayoutIncludesOpaqueBars = false;
            //AutomaticallyAdjustsScrollViewInsets = false;
            

			tableview.RegisterClassForCellReuse( typeof(UITableViewCell), @"cell");
			tableview.Source = new TimeSheetsOverviewTableViewSource (this);
			tableview.ReloadData ();
			//Add ( tableview );

			NavigationController.NavigationBar.Translucent = false;
		}
	}
}
