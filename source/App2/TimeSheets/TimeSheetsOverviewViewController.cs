using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
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
		}
	}
}
