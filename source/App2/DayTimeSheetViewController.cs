using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class DayTimeSheetViewController : UIViewController
	{
		public DayTimeSheetViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableview.RegisterClassForCellReuse( typeof(UITableViewCell), @"cell");
            tableview.Source = new TimeEntryTableViewSource(this);
            tableview.ReloadData();

            addEntryButton.TouchUpInside += delegate
            {
               
            };
        }
	}
}
