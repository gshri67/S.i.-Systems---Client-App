using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class DayTimeSheetViewController : UIViewController
	{
        private List<String> clientNames;

		public DayTimeSheetViewController (IntPtr handle) : base (handle)
		{
            clientNames = new List<String>();
            clientNames.Add("Nexen");
            clientNames.Add("Cenovus");

		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableview.RegisterClassForCellReuse( typeof(UITableViewCell), @"cell");
            tableview.Source = new TimeEntryTableViewSource(this, clientNames );
            tableview.ReloadData();

            addEntryButton.TouchUpInside += delegate
            {
                clientNames.Add("New Client");
                tableview.ReloadData();
                tableview.BeginUpdates();
                tableview.EndUpdates();
            };
        }
	}
}
