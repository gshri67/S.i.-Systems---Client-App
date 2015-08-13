using System;
using UIKit;
using ConsultantApp.iOS.TimeEntryViewController;

namespace ConsultantApp.iOS.Review_TimeSheets
{
	partial class ActiveTimesheetViewController : UIViewController
	{
		public ActiveTimesheetViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//tableview = new UITableView(View.Bounds);

			//EdgesForExtendedLayout = UIRectEdge.None;
			//ExtendedLayoutIncludesOpaqueBars = false;
			//AutomaticallyAdjustsScrollViewInsets = false;


			tableview.RegisterClassForCellReuse( typeof(ActiveTimesheetCell), @"reviewTimeSheetCell");
			tableview.Source = new ActiveTimesheetTableViewSource (this);
			tableview.ReloadData ();
			//Add ( tableview );

			NavigationController.NavigationBar.Translucent = false;

			//TimesheetOverviewViewController vc = (TimesheetOverviewViewController)Storyboard.InstantiateViewController ("TimesheetOverviewViewController");

			//if( vc != null )
			//	NavigationController.PushViewController ( vc, true );
		}
	}
}
