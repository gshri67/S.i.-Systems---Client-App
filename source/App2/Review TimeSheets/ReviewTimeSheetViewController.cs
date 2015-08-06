using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class ReviewTimeSheetViewController : UIViewController
	{
		public ReviewTimeSheetViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//tableview = new UITableView(View.Bounds);

			//EdgesForExtendedLayout = UIRectEdge.None;
			//ExtendedLayoutIncludesOpaqueBars = false;
			//AutomaticallyAdjustsScrollViewInsets = false;


			tableview.RegisterClassForCellReuse( typeof(ReviewTimeSheetCell), @"reviewTimeSheetCell");
			tableview.Source = new ReviewTimeSheetsTableViewSource (this);
			tableview.ReloadData ();
			//Add ( tableview );

			NavigationController.NavigationBar.Translucent = false;
		}
	}
}
