using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class MultiSelectTableViewSource : UITableViewSource
    {
        private readonly MultiSelectTableViewController _parentController;

        public string[] ListTitles = new string[0];

        public MultiSelectTableViewSource(MultiSelectTableViewController parentController)
        {
            _parentController = parentController;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("UITableViewCell") as UITableViewCell;

            cell.TextLabel.Text = ListTitles[(int)indexPath.Item];

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if( ListTitles != null )
                return ListTitles.Length;
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}