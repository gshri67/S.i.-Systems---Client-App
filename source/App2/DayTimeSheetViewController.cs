using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.CodeDom.Compiler;
using UIKit;
using ConsultantApp.SharedModels;
using ConsultantApp.Core;
using ConsultantApp.Core.ViewModels;

namespace App2
{
	partial class DayTimeSheetViewController : UIViewController
	{
        private List<TimeEntry> clientNames;
        public TimeEntryViewModel viewModel;

		public DayTimeSheetViewController (IntPtr handle) : base (handle)
		{
            viewModel = new TimeEntryViewModel();

            clientNames = viewModel.getTimeEntries(new DateTime());
            clientNames.Add( new TimeEntry("Nexen", "PC123", 4) );
            clientNames.Add( new TimeEntry("Cenovus", "PC456", 3) );

			EdgesForExtendedLayout = UIRectEdge.None;
			//NavigationController.ExtendedLayoutIncludesOpaqueBars = true;
			//NavigationController.EdgesForExtendedLayout = UIRectEdge.None;

		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //tableview.RegisterClassForCellReuse( typeof(UITableViewCell), @"cell");
            tableview.RegisterClassForCellReuse(typeof(TimeEntryCell), @"TimeEntryCell");
            tableview.Source = new TimeEntryTableViewSource(this, clientNames );
            tableview.ReloadData();

            addEntryButton.TouchUpInside += delegate
            {
                clientNames.Add( new TimeEntry("New Client", "New Project Code", 0) );
                tableview.ReloadData();
                //tableview.BeginUpdates();
                //tableview.EndUpdates();
            };
        }
	}
}
