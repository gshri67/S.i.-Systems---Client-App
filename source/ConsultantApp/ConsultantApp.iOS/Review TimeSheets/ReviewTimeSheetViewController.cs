using System;
using UIKit;

namespace ConsultantApp.iOS.Review_TimeSheets
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
